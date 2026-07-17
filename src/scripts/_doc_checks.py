#!/usr/bin/env python3
"""
_doc_checks.py — two-phase doc consistency checker for Project Babel.
Phase 1: structure checks (segment count/level, CJK residue, crosslinks) — no API cost.
Phase 2: LLM semantic comparison — only runs if Phase 1 passes AND --full flag is set.

Usage:
    python src/scripts/_doc_checks.py              # Phase 1 only (dry-run)
    python src/scripts/_doc_checks.py --full       # Phase 1 + Phase 2
    python src/scripts/_doc_checks.py --full --family readme  # single family
"""

import subprocess
import sys
import argparse
from pathlib import Path


SCRIPTS_DIR = Path(__file__).resolve().parent
PROJECT_ROOT = SCRIPTS_DIR.parent.parent


def run_step(cmd: list[str], label: str) -> bool:
    """Run a subprocess check. Returns True if passed (rc=0). Output streams live."""
    print(f"\n--- {label} ---")
    r = subprocess.run(cmd, cwd=PROJECT_ROOT)
    if r.returncode == 0:
        print(f"--- {label}: PASSED ---")
    else:
        print(f"--- {label}: FAILED (exit={r.returncode}) ---")
    return r.returncode == 0


def main():
    parser = argparse.ArgumentParser(
        description="Two-phase doc consistency checker"
    )
    parser.add_argument(
        "--full", action="store_true",
        help="Run Phase 2 (LLM semantic comparison) if Phase 1 passes"
    )
    parser.add_argument(
        "--family", type=str, default="",
        help="Comma-separated doc families (technical_reference,readme,contributing). Default: all"
    )
    args = parser.parse_args()

    family_args = ["--family", args.family] if args.family else []
    python_exe = sys.executable

    # ── Phase 1: Structure Checks ────────────────────────────
    print("=" * 50)
    print("Phase 1: Structure Checks")
    print("=" * 50)
    print(f"Coverage: technical_reference / readme / contributing")
    if not args.full:
        print("Mode: dry-run (use --full for LLM comparison)")
    else:
        print("Mode: full (Phase 2 will run if Phase 1 passes)")

    phase1_fail = 0
    soft_warnings = 0

    # 1a — segment structure (硬性, 阻断 Phase 2)
    ok = run_step(
        [python_exe, str(SCRIPTS_DIR / "_list_segments.py")] + family_args,
        "1/3 Segment Structure"
    )
    if not ok:
        phase1_fail += 1

    # 1b — CJK residue (软警告, 不阻断)
    ok = run_step(
        [python_exe, str(SCRIPTS_DIR / "_find_cjk.py")],
        "2/3 CJK Residue Scan (non-blocking)"
    )
    if not ok:
        soft_warnings += 1

    # 1c — crosslinks (软警告, 不阻断)
    ok = run_step(
        [python_exe, str(SCRIPTS_DIR / "_add_crosslinks.py"), "--check"],
        "3/3 Crosslink Check (non-blocking)"
    )
    if not ok:
        soft_warnings += 1

    # ── Phase 1 result ───────────────────────────────────────
    print()
    if phase1_fail > 0:
        print(f"Phase 1 FAILED — {phase1_fail} structural check(s) had issues.")
        print("Fix the above before re-running with --full.")
        sys.exit(1)

    if soft_warnings > 0:
        print(f"Phase 1 PASSED — {soft_warnings} non-blocking warning(s) above (CJK / crosslinks).")
    else:
        print("Phase 1: ALL PASSED")

    if not args.full:
        print("Dry-run complete. Use --full to enable Phase 2 (LLM semantic comparison).")
        sys.exit(0)

    # ── Phase 2: LLM Semantic Comparison ─────────────────────
    print()
    print("=" * 50)
    print("Phase 2: LLM Semantic Comparison")
    print("=" * 50)
    print("Structures verified — starting LLM comparison...")

    r = subprocess.run(
        [python_exe, str(SCRIPTS_DIR / "_compare_docs.py")] + family_args,
        cwd=PROJECT_ROOT
    )
    if r.returncode != 0:
        print("\nPhase 2 found issues — see output above and report file.")
    else:
        print("\nPhase 2: ALL PASSED")
    sys.exit(r.returncode)


if __name__ == "__main__":
    main()

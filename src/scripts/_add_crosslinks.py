#!/usr/bin/env python3
"""
src/scripts/_add_crosslinks.py — 为所有文档添加多语言交叉连接
覆盖 technical_reference, readme, contributing 三个文档家族。
在 `---` 分隔线和第一个 `##` 标题之间插入语言跳转栏。
"""

import re
from pathlib import Path

BASE_DIR = Path(__file__).resolve().parent.parent.parent

# iso → native name
LANG_NAMES = {
    "ar": "العربية", "ca": "català", "cs": "čeština", "da": "dansk",
    "de": "Deutsch", "en": "English", "es": "español", "fi": "suomi",
    "fr": "français", "hu": "magyar", "id": "Bahasa Indonesia",
    "it": "italiano", "ja": "日本語", "ko": "한국어",
    "nl": "Nederlands", "no": "norsk", "pl": "polski",
    "pt": "português", "pt-br": "português do Brasil", "ro": "română",
    "ru": "русский", "th": "ไทย", "tl": "Tagalog", "tr": "Türkçe",
    "uk": "українська", "zh-hans": "简体中文", "zh-hant": "繁體中文",
}

# Order for the link list (alphabetical by ISO, same as file listing)
LINK_ORDER = [
    "ar", "ca", "zh-hant", "cs", "da", "de", "en", "es", "fi", "fr",
    "hu", "id", "it", "ja", "ko", "nl", "no", "tl", "pl", "pt",
    "pt-br", "ro", "ru", "th", "tr", "uk", "zh-hans",
]

# ── doc families ────────────────────────────────────────────
FAMILIES = [
    {
        "dir": BASE_DIR / "docs" / "technical_reference",
        "glob": "technical_reference_*.md",
        "prefix": "technical_reference_",
        "label": "技术文档",
    },
    {
        "dir": BASE_DIR / "docs" / "readme",
        "glob": "README_*.md",
        "prefix": "README_",
        "label": "README",
        "crosslink_overrides": {"zh-hans": "../README.md"},
    },
    {
        "dir": BASE_DIR / "docs" / "contributing",
        "glob": "contributing_*.md",
        "prefix": "contributing_",
        "label": "贡献指南",
    },
]

def build_other_langs(exclude: set, prefix: str, overrides: dict | None = None) -> str:
    """Build the | separated link list for <details>, excluding given isos."""
    parts = []
    for iso in LINK_ORDER:
        if iso in exclude:
            continue
        fname = overrides.get(iso) if overrides else None
        if fname is None:
            fname = f"{prefix}{iso}.md"
        native = LANG_NAMES[iso]
        parts.append(f"[{native}]({fname})")
    return " | ".join(parts)

def build_crosslink(current_iso: str, prefix: str, overrides: dict | None = None) -> str:
    """Build the full cross-link block for a given language file."""
    primary_parts = []

    if current_iso != "zh-hans":
        zh_link = overrides.get("zh-hans") if overrides else None
        if zh_link is None:
            zh_link = f"{prefix}zh-hans.md"
        primary_parts.append(f"[简体中文]({zh_link})")
    if current_iso != "en":
        primary_parts.append(f"[English]({prefix}en.md)")

    other_exclude = {"zh-hans", "en", current_iso}
    others = build_other_langs(other_exclude, prefix, overrides)

    primary = " ".join(primary_parts)
    return f"> {primary} <details><summary>Other Languages</summary>{others}</details>\n"

def process_file(filepath: Path, prefix: str, overrides: dict | None = None):
    text = filepath.read_text(encoding="utf-8")
    iso = filepath.stem.replace(prefix, "")

    crosslink = build_crosslink(iso, prefix, overrides)

    if "<details><summary>Other Languages</summary>" in text:
        print(f"  [{iso}] SKIP (already has cross-links)")
        return

    m = re.search(r"\n---\n\n", text)
    if not m:
        print(f"  [{iso}] SKIP (no --- separator found)")
        return

    insert_pos = m.end()
    new_text = text[:insert_pos] + crosslink + text[insert_pos:]

    filepath.write_text(new_text, encoding="utf-8")
    print(f"  [{iso}] DONE")

def main():
    for family in FAMILIES:
        files = sorted(family["dir"].glob(family["glob"]))
        overrides = family.get("crosslink_overrides")
        print(f"\n--- {family['label']} ({family['dir'].name}) ---")
        print(f"  {len(files)} file(s)")
        for f in files:
            process_file(f, family["prefix"], overrides)
    print("\nDone.")

if __name__ == "__main__":
    main()

#!/usr/bin/env python3
"""
src/scripts/_find_cjk.py — 扫描所有文档翻译中的 CJK 字符残留
覆盖 docs/technical_reference/, docs/readme/, docs/contributing/ 下非中文翻译文件，
检测代码块和表格分隔线以外的中文字符残留。
"""

import re
from pathlib import Path

BASE_DIR = Path(__file__).resolve().parent.parent.parent  # project_babel root
CJK_RE = re.compile(r'[\u4e00-\u9fff\u3400-\u4dbf\uf900-\ufaff]')

FAMILIES = [
    {
        "dir": BASE_DIR / "docs" / "technical_reference",
        "glob": "technical_reference_*.md",
        "skip": {"technical_reference_zh-hans.md", "technical_reference_zh-hant.md"},
        "label": "技术文档",
    },
    {
        "dir": BASE_DIR / "docs" / "readme",
        "glob": "README_*.md",
        "skip": {"README_zh-hans.md", "README_zh-hant.md"},
        "label": "README",
    },
    {
        "dir": BASE_DIR / "docs" / "contributing",
        "glob": "contributing_*.md",
        "skip": {"contributing_zh-hans.md", "contributing_zh-hant.md"},
        "label": "贡献指南",
    },
]

for family in FAMILIES:
    doc_dir = family["dir"]
    files = sorted(doc_dir.glob(family["glob"]))
    print(f"\n--- {family['label']} ({doc_dir.name}) ---")
    found_any = False
    for f in files:
        if f.name in family["skip"]:
            continue
        text = f.read_text(encoding='utf-8')
        lines = text.split('\n')
        issues = []
        for i, line in enumerate(lines, 1):
            stripped = line.strip()
            if stripped.startswith('```') or stripped.startswith('|---') or stripped.startswith('|--'):
                continue
            matches = CJK_RE.findall(line)
            if matches:
                issues.append((i, ''.join(matches), line.strip()[:120]))
        if issues:
            lang = f.stem
            # extract iso: e.g. technical_reference_hu → hu, README_hu → hu, contributing_hu → hu
            for prefix in ("technical_reference_", "README_", "contributing_"):
                if lang.startswith(prefix):
                    lang = lang[len(prefix):]
                    break
            print(f'\n[{lang}] {len(issues)} line(s) with CJK:')
            for ln, chars, ctx in issues[:20]:
                print(f'  L{ln}: [{chars}] {ctx}')
            found_any = True
    if not found_any:
        print(f"  (无 CJK 残留)")

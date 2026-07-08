#!/usr/bin/env python3
"""
src/scripts/_find_cjk.py — 扫描技术文档翻译中的 CJK 字符残留
扫描 docs/technical_reference/ 下所有非中文翻译文件，
检测代码块和表格分隔线以外的中文字符残留。
"""

import re
from pathlib import Path

BASE_DIR = Path(__file__).resolve().parent.parent.parent  # project_babel root
DOC_DIR = BASE_DIR / "docs" / "technical_reference"
CJK_RE = re.compile(r'[\u4e00-\u9fff\u3400-\u4dbf\uf900-\ufaff]')

skip = {'technical_reference_zh-hans.md', 'technical_reference_zh-hant.md'}

for f in sorted(DOC_DIR.glob('technical_reference_*.md')):
    if f.name in skip:
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
        lang = f.stem.replace('technical_reference_', '')
        print(f'\n[{lang}] {len(issues)} line(s) with CJK:')
        for ln, chars, ctx in issues[:20]:
            print(f'  L{ln}: [{chars}] {ctx}')

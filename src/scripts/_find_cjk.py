#!/usr/bin/env python3
"""
src/scripts/_find_cjk.py — 扫描文档翻译中的 CJK 字符残留
输出有问题文件作为警告, 始终 exit 0 (不阻断后续流程)
"""

import re, sys
from pathlib import Path

# Windows 控制台 UTF-8 兼容
if sys.stdout.encoding != 'utf-8':
    sys.stdout.reconfigure(encoding='utf-8', errors='replace')

BASE_DIR = Path(__file__).resolve().parent.parent.parent  # project_babel root
CJK_RE = re.compile(r'[\u4e00-\u9fff\u3400-\u4dbf\uf900-\ufaff]')

FAMILIES = [
    {
        "dir": BASE_DIR / "docs" / "technical_reference",
        "glob": "technical_reference_*.md",
        "skip": {"technical_reference_zh-hans.md"},
        "label": "技术文档",
    },
    {
        "dir": BASE_DIR / "docs" / "readme",
        "glob": "README_*.md",
        "skip": {"README_zh-hans.md"},
        "label": "README",
    },
    {
        "dir": BASE_DIR / "docs" / "contributing",
        "glob": "contributing_*.md",
        "skip": {"contributing_zh-hans.md"},
        "label": "贡献指南",
    },
]

total_issues = 0
for family in FAMILIES:
    doc_dir = family["dir"]
    files = sorted(doc_dir.glob(family["glob"]))
    for f in files:
        if f.name in family["skip"]:
            continue
        text = f.read_text(encoding='utf-8')
        lines = text.split('\n')
        issues = []
        in_crosslink_block = False
        for i, line in enumerate(lines, 1):
            stripped = line.strip()
            # 跳过交叉连接触发行 (含语言链接的 > 行)
            if stripped.startswith('>') and '<details><summary>' in stripped:
                continue
            # 跳过交叉连接块内部
            if '<details><summary>' in stripped:
                in_crosslink_block = True
                continue
            if in_crosslink_block and '</details>' in stripped:
                in_crosslink_block = False
                continue
            if in_crosslink_block:
                continue
            # 跳过代码块和表格分隔线
            if stripped.startswith('```') or stripped.startswith('|---') or stripped.startswith('|--'):
                continue
            matches = CJK_RE.findall(line)
            if matches:
                issues.append((i, ''.join(matches), line.strip()[:120]))
        if issues:
            lang = f.stem
            for prefix in ("technical_reference_", "README_", "contributing_"):
                if lang.startswith(prefix):
                    lang = lang[len(prefix):]
                    break
            print(f'\n[{family["label"]}] [{lang}] {len(issues)} line(s) with CJK:')
            for ln, chars, ctx in issues[:20]:
                # 安全打印, 替换不可编码字符
                try:
                    print(f'  L{ln}: [{chars}] {ctx}')
                except UnicodeEncodeError:
                    print(f'  L{ln}: [{chars}] <encoding error, see file>')
            total_issues += len(issues)

if total_issues > 0:
    print(f"\n=== CJK 残留: {total_issues} 处 ===")
# CJK 为软警告, 始终 exit 0 不阻断后续流程

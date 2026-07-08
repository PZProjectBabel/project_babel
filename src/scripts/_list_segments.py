#!/usr/bin/env python3
"""
src/scripts/_list_segments.py — 列出技术文档的 Markdown 段落映射
按标题切分文档，输出每个段落的索引、行范围和标题。
用法: python src/scripts/_list_segments.py [filepath]
      python src/scripts/_list_segments.py docs/technical_reference/technical_reference_hu.md
"""

import re, sys
from pathlib import Path

BASE_DIR = Path(__file__).resolve().parent.parent.parent  # project_babel root

def list_segments(filepath):
    text = Path(filepath).read_text(encoding='utf-8')
    lines = text.split('\n')
    HEADING_RE = re.compile(r'^(#{1,6}\s)', re.MULTILINE)

    heading_indices = []
    in_fence = False
    for i, ln in enumerate(lines):
        stripped = ln.strip()
        if stripped.startswith('```'):
            in_fence = not in_fence
            continue
        if not in_fence and HEADING_RE.match(ln):
            heading_indices.append(i)

    for j, h_idx in enumerate(heading_indices):
        start = h_idx
        end = heading_indices[j + 1] - 1 if j + 1 < len(heading_indices) else len(lines) - 1
        while end > start and lines[end].strip() == '':
            end -= 1
        heading = lines[start].strip()
        lcount = end - start + 1
        print(f'seg[{j:03d}] L{start+1}-L{end+1} ({lcount} lines) | {heading}')

if __name__ == '__main__':
    if len(sys.argv) > 1:
        list_segments(sys.argv[1])
    else:
        list_segments(BASE_DIR / 'docs' / 'technical_reference' / 'technical_reference_zh-hans.md')

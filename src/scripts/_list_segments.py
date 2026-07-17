#!/usr/bin/env python3
"""
src/scripts/_list_segments.py — 文档段落结构对比
默认: 比较各语种与 zh-hans 基准的段落数量/标题, 只输出不一致
--file <path>: 列出单文件段落(兼容旧用法)
--family <name>: 限定文档家族
"""

import re, sys
from pathlib import Path

if sys.stdout.encoding != 'utf-8':
    sys.stdout.reconfigure(encoding='utf-8', errors='replace')

BASE_DIR = Path(__file__).resolve().parent.parent.parent

DOC_FAMILIES = {
    "technical_reference": {
        "dir": BASE_DIR / "docs" / "technical_reference",
        "base": "technical_reference_zh-hans.md",
        "glob": "technical_reference_*.md",
        "skip": {"technical_reference_zh-hans.md", "technical_reference_zh-hant.md"},
        "prefix": "technical_reference_",
        "label": "技术文档",
    },
    "readme": {
        "dir": BASE_DIR / "docs" / "readme",
        "base": "README_zh-hans.md",
        "base_path": BASE_DIR / "README.md",  # 中文 README 在仓库根目录
        "glob": "README_*.md",
        "skip": {"README_zh-hans.md", "README_zh-hant.md"},
        "prefix": "README_",
        "label": "README",
    },
    "contributing": {
        "dir": BASE_DIR / "docs" / "contributing",
        "base": "contributing_zh-hans.md",
        "glob": "contributing_*.md",
        "skip": {"contributing_zh-hans.md", "contributing_zh-hant.md"},
        "prefix": "contributing_",
        "label": "贡献指南",
    },
}

HEADING_RE = re.compile(r'^(#{1,6}\s)', re.MULTILINE)


def get_segments(filepath):
    """返回 [(line_1based, heading_level, heading_text), ...]"""
    text = Path(filepath).read_text(encoding='utf-8')
    lines = text.split('\n')
    heading_indices = []
    in_fence = False
    for i, ln in enumerate(lines):
        stripped = ln.strip()
        if stripped.startswith('```'):
            in_fence = not in_fence
            continue
        if not in_fence and HEADING_RE.match(ln):
            heading_indices.append(i)

    segments = []
    for h_idx in heading_indices:
        heading = lines[h_idx].strip()
        level = len(heading) - len(heading.lstrip('#'))
        segments.append((h_idx + 1, level, heading))
    return segments


def check_family(family):
    """比较家族内所有目标文件 vs zh-hans, 返回不一致列表"""
    base_file = family.get("base_path") or (family["dir"] / family["base"])
    if not base_file.exists():
        return [f"[{family['label']}] 基准文件缺失: {base_file}"]

    zh_segs = get_segments(base_file)
    targets = sorted([f for f in family["dir"].glob(family["glob"])
                      if f.name not in family["skip"]])

    issues = []
    for tf in targets:
        iso = tf.stem.replace(family["prefix"], "")
        tgt_segs = get_segments(tf)

        if len(zh_segs) != len(tgt_segs):
            issues.append(
                f"[{family['label']}] [{iso}] 段落数不一致: "
                f"zh={len(zh_segs)} tgt={len(tgt_segs)}"
            )
            # 段落数不一致可能导致后续全错位，只输出这一项，跳下一个文件
            continue

        for i in range(len(zh_segs)):
            zh_lvl = zh_segs[i][1]
            tgt_lvl = tgt_segs[i][1]
            if zh_lvl != tgt_lvl:
                zh_h = zh_segs[i][2]
                tgt_h = tgt_segs[i][2]
                issues.append(
                    f"[{family['label']}] [{iso}] seg[{i:03d}] 标题级别不一致 | "
                    f"zh(L{zh_lvl}): {zh_h[:60]} | tgt(L{tgt_lvl}): {tgt_h[:60]}"
                )
                # 第一个不一致跳出，避免级联误报，跳下一个文件
                break

    return issues


def list_single_file(filepath):
    """旧行为: 列出单文件段落"""
    segs = get_segments(filepath)
    text = Path(filepath).read_text(encoding='utf-8')
    lines = text.split('\n')
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
    import argparse
    parser = argparse.ArgumentParser()
    parser.add_argument("--family", type=str, default="",
                        help="逗号分隔的文档家族")
    parser.add_argument("--file", type=str, default="",
                        help="单文件路径(兼容旧用法, 列出段落)")
    args = parser.parse_args()

    if args.file:
        list_single_file(args.file)
        sys.exit(0)

    families = (args.family.split(",") if args.family
                else list(DOC_FAMILIES.keys()))

    all_issues = []
    for fam_name in families:
        if fam_name not in DOC_FAMILIES:
            print(f"[WARN] 未知家族: {fam_name}")
            continue
        all_issues.extend(check_family(DOC_FAMILIES[fam_name]))

    if all_issues:
        print(f"\n=== 结构不一致 ({len(all_issues)} 项) ===")
        for issue in all_issues:
            print(f"  {issue}")
        sys.exit(1)
    # else: 无输出 = 全部一致

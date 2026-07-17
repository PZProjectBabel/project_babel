#!/usr/bin/env python3
"""
src/scripts/_add_crosslinks.py — 多语言交叉连接检查/添加
默认 --check: 只报告缺少交叉连接的文件, 不修改
去掉 --check: 实际添加交叉连接
"""

import re, sys
from pathlib import Path

if sys.stdout.encoding != 'utf-8':
    sys.stdout.reconfigure(encoding='utf-8', errors='replace')

BASE_DIR = Path(__file__).resolve().parent.parent.parent

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

LINK_ORDER = [
    "ar", "ca", "zh-hant", "cs", "da", "de", "en", "es", "fi", "fr",
    "hu", "id", "it", "ja", "ko", "nl", "no", "tl", "pl", "pt",
    "pt-br", "ro", "ru", "th", "tr", "uk", "zh-hans",
]

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


def process_file(filepath: Path, prefix: str, overrides: dict | None = None, check_only: bool = False):
    text = filepath.read_text(encoding="utf-8")
    iso = filepath.stem.replace(prefix, "")

    if "<details><summary>" in text:
        return None  # 已有, 无问题

    if check_only:
        return f"[{iso}] 缺少交叉连接"

    crosslink = build_crosslink(iso, prefix, overrides)
    m = re.search(r"\n---\n\n", text)
    if not m:
        return f"[{iso}] 缺少 --- 分隔线, 无法添加"

    insert_pos = m.end()
    new_text = text[:insert_pos] + crosslink + text[insert_pos:]
    filepath.write_text(new_text, encoding="utf-8")
    return None  # 已修复, 无输出


def main():
    import argparse
    parser = argparse.ArgumentParser()
    parser.add_argument("--check", action="store_true",
                        help="只检查不修改(默认)")
    parser.add_argument("--fix", action="store_true",
                        help="实际添加交叉连接")
    args = parser.parse_args()

    check_only = not args.fix  # 默认 check 模式
    all_issues = []

    for family in FAMILIES:
        files = sorted(family["dir"].glob(family["glob"]))
        overrides = family.get("crosslink_overrides")
        for f in files:
            result = process_file(f, family["prefix"], overrides, check_only)
            if result:
                all_issues.append(f"[{family['label']}] {result}")

    if all_issues:
        action = "缺失" if check_only else "修复"
        print(f"\n=== 交叉连接{action} ({len(all_issues)} 项) ===")
        for issue in all_issues:
            print(f"  {issue}")
        if check_only:
            sys.exit(1)
    # else: 无输出 = 全部一致


if __name__ == "__main__":
    main()

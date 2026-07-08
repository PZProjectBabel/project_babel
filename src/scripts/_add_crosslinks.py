#!/usr/bin/env python3
"""
src/scripts/_add_crosslinks.py — 为所有技术文档添加多语言交叉连接
在 `---` 分隔线和第一个 `##` 标题之间插入语言跳转栏。
"""

import re
from pathlib import Path

BASE_DIR = Path(__file__).resolve().parent.parent.parent
DOC_DIR = BASE_DIR / "docs" / "technical_reference"

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

def build_other_langs(exclude: set) -> str:
    """Build the | separated link list for <details>, excluding given isos."""
    parts = []
    for iso in LINK_ORDER:
        if iso in exclude:
            continue
        fname = f"technical_reference_{iso}.md"
        native = LANG_NAMES[iso]
        parts.append(f"[{native}]({fname})")
    return " | ".join(parts)

def build_crosslink(current_iso: str) -> str:
    """Build the full cross-link block for a given language file."""
    primary_parts = []

    # zh-hans: primary link to en only
    # en: primary link to zh-hans only
    # other: primary links to both zh-hans and en
    exclude = {current_iso}

    if current_iso != "zh-hans":
        primary_parts.append(f"[简体中文](technical_reference_zh-hans.md)")
    if current_iso != "en":
        primary_parts.append(f"[English](technical_reference_en.md)")

    # Other Languages excludes zh-hans, en, and self
    other_exclude = {"zh-hans", "en", current_iso}
    others = build_other_langs(other_exclude)

    primary = " ".join(primary_parts)
    return f"> {primary} <details><summary>Other Languages</summary>{others}</details>\n"

def process_file(filepath: Path):
    text = filepath.read_text(encoding="utf-8")
    iso = filepath.stem.replace("technical_reference_", "")

    crosslink = build_crosslink(iso)

    # Check if cross-link already exists
    if "<details><summary>Other Languages</summary>" in text:
        print(f"  [{iso}] SKIP (already has cross-links)")
        return

    # Insert after the first `---\n` that follows the metadata block
    # Pattern: # Title\n> meta...\n---\n## First heading
    m = re.search(r"\n---\n\n", text)
    if not m:
        print(f"  [{iso}] SKIP (no --- separator found)")
        return

    insert_pos = m.end()
    new_text = text[:insert_pos] + crosslink + text[insert_pos:]

    filepath.write_text(new_text, encoding="utf-8")
    print(f"  [{iso}] DONE")

def main():
    files = sorted(DOC_DIR.glob("technical_reference_*.md"))
    print(f"Processing {len(files)} files...")
    for f in files:
        process_file(f)
    print("Done.")

if __name__ == "__main__":
    main()

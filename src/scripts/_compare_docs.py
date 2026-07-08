#!/usr/bin/env python3
"""
src/scripts/_compare_docs.py — 自动切分+LLM对比+二次核对 技术文档多语种差异
基准: technical_reference_zh-hans.md
目标: 同目录下其他语种文件
用法: python src/scripts/_compare_docs.py [--dry-run] [--lang hu,ja,ko]
"""

import re, json, os, sys, time, argparse
from pathlib import Path
from concurrent.futures import ThreadPoolExecutor, as_completed
import requests

# ── config ──────────────────────────────────────────────────
BASE_DIR    = Path(__file__).resolve().parent.parent.parent  # project_babel root
DOC_DIR     = BASE_DIR / "docs" / "technical_reference"
BASE_FILE   = "technical_reference_zh-hans.md"
SECRETS     = json.loads((BASE_DIR / "config" / "secrets.json").read_text(encoding="utf-8"))
CONFIG      = json.loads((BASE_DIR / "config" / "config.json").read_text(encoding="utf-8"))

LLM_KEY      = SECRETS["LLM_KEY"]
LLM_ENDPOINT = CONFIG["LLM"]["api_endpoint"]
LLM_MODEL    = "deepseek-v4-flash"
LLM_TIMEOUT  = 120
MAX_CONCUR   = 16

# ── lang name mapping (iso→native) ──────────────────────────
LANG_NAMES = {
    "ar":"العربية","ca":"català","cs":"čeština","da":"dansk","de":"Deutsch",
    "es":"español","fi":"suomi","fr":"français","hu":"magyar","id":"Bahasa Indonesia",
    "it":"italiano","ja":"日本語","ko":"한국어","nl":"Nederlands","no":"norsk",
    "pl":"polski","pt":"português","pt-br":"português (Brasil)","ro":"română",
    "ru":"русский","th":"ไทย","tl":"Tagalog","tr":"Türkçe","uk":"українська",
    "zh-hant":"繁體中文",
}

# ── util ────────────────────────────────────────────────────
def iso_from_filename(fname: str) -> str:
    """technical_reference_hu.md → hu"""
    return fname.replace("technical_reference_", "").replace(".md", "")

def lang_name(iso: str) -> str:
    return LANG_NAMES.get(iso, iso)

# ── split ───────────────────────────────────────────────────
HEADING_RE = re.compile(r"^(#{1,6}\s)", re.MULTILINE)

def split_by_headings(text: str):
    """
    按md标题切分. 每段=标题行+其后所有行直到下一标题.
    跳过代码围栏（```）内部的 # 行，避免误切分。
    返回 [(start_line_1based, end_line_1based, content), ...]
    """
    lines = text.split("\n")
    # 找所有标题行号(0-based)，跳过代码围栏内部
    heading_indices = []
    in_fence = False
    for i, ln in enumerate(lines):
        stripped = ln.strip()
        # 检测代码围栏边界
        if stripped.startswith("```"):
            in_fence = not in_fence
            continue
        if not in_fence and HEADING_RE.match(ln):
            heading_indices.append(i)

    if not heading_indices:
        return [(1, len(lines), text)]

    segments = []
    for j, h_idx in enumerate(heading_indices):
        start = h_idx
        end = heading_indices[j + 1] - 1 if j + 1 < len(heading_indices) else len(lines) - 1
        # 去掉尾部空行
        while end > start and lines[end].strip() == "":
            end -= 1
        seg_text = "\n".join(lines[start:end + 1])
        segments.append((start + 1, end + 1, seg_text))  # 1-based

    return segments

# ── structural landmarks ────────────────────────────────────
def structural_landmarks(text: str):
    """
    返回 {type: [line_numbers_1based]} 的结构特征:
    - blank: 空行
    - code_fence: ``` 或 ```lang 行
    - brace_line: 只含花括号的行
    """
    lines = text.split("\n")
    out = {"blank": [], "code_fence": [], "brace_line": []}
    for i, ln in enumerate(lines):
        ln1 = i + 1
        if ln.strip() == "":
            out["blank"].append(ln1)
        if re.match(r"^```", ln.strip()):
            out["code_fence"].append(ln1)
        if re.match(r"^[{}]\s*$", ln.strip()):
            out["brace_line"].append(ln1)
    return out

# ── LLM call ────────────────────────────────────────────────
PROMPT_TMPL = """\
你是语义对比机。只输出两行：
第一行: true 或 false (全部通过=true, 否则=false)
第二行: 简短原因(一行, 多问题用;分隔, 指出具体位置)

检查项(任一不通过则false):
A. 整体语义是否与中文原文一致
B. 目标语言段落中是否有未翻译的英文/中文残留 (注意区分: 代码块内容/变量名/函数名/类名/文件名/路径/URL/Steam ID/专有名词/API字段名 不算残留)
C. Markdown结构是否完整 (代码块```是否成对, 表格列数是否一致)

对比: 中文(原文) vs {target_lang}(目标语言)

<中文片段起始>
{zh_text}
</中文片段结束>
<目标语言片段起始>
{tgt_text}
</目标语言片段结束>"""

def call_llm(zh_text: str, tgt_text: str, target_iso: str):
    prompt = PROMPT_TMPL.format(
        target_lang=f"{target_iso}",
        zh_text=zh_text,
        tgt_text=tgt_text,
    )
    payload = {
        "model": LLM_MODEL,
        "messages": [{"role": "user", "content": prompt}],
        "temperature": 0.0,
        "max_tokens": 4096,
    }
    headers = {
        "Authorization": f"Bearer {LLM_KEY}",
        "Content-Type": "application/json",
    }
    try:
        r = requests.post(LLM_ENDPOINT, json=payload, headers=headers, timeout=LLM_TIMEOUT)
        r.raise_for_status()
        body = r.json()
        msg = body["choices"][0]["message"]
        raw = (msg.get("content") or msg.get("reasoning_content") or "").strip().lower()
        return raw
    except Exception as e:
        return f"ERROR:{e}"

def parse_llm(raw: str, debug_label: str = ""):
    """解析LLM返回的true/false. 全文搜索."""
    if raw.startswith("ERROR:"):
        return None, raw
    # 取最后一行非空内容 (模型可能在前面推理)
    lines = [l.strip().lower() for l in raw.split("\n") if l.strip()]
    for ln in reversed(lines):
        if ln in ("true", "false"):
            return ln == "true", raw
        if ln.startswith("true"):
            return True, raw
        if ln.startswith("false"):
            return False, raw
    # fallback: 全文搜索
    if "true" in raw.lower():
        return True, raw
    if "false" in raw.lower():
        return False, raw
    if debug_label:
        print(f"  [DEBUG] parse_llm FAIL for {debug_label}, raw={raw[:300]}")
    return None, raw

# ── verify ──────────────────────────────────────────────────
def verify_segment(zh_text: str, tgt_text: str, llm_semantic: bool | None):
    """
    纯程序检查: 行数+结构
    llm_semantic: LLM语义判断(true=一致, false=不一致, None=解析失败)
    """
    zh_lines = zh_text.count("\n") + 1
    tgt_lines = tgt_text.count("\n") + 1
    line_match = (zh_lines == tgt_lines)
    line_verdict = "OK" if line_match else f"MISMATCH: zh={zh_lines} tgt={tgt_lines}"

    # 结构特征
    zh_s = structural_landmarks(zh_text)
    tgt_s = structural_landmarks(tgt_text)
    struct_diffs = []
    for k in ["blank", "code_fence", "brace_line"]:
        if zh_s[k] != tgt_s[k]:
            struct_diffs.append(f"{k}: zh={zh_s[k]} tgt={tgt_s[k]}")

    return {
        "zh_lines": zh_lines,
        "tgt_lines": tgt_lines,
        "line_match": line_match,
        "line_verdict": line_verdict,
        "llm_semantic": llm_semantic,
        "struct_match": len(struct_diffs) == 0,
        "struct_diffs": struct_diffs,
    }

# ── process one target lang ─────────────────────────────────
def process_lang(target_file: Path):
    iso = iso_from_filename(target_file.name)
    name = lang_name(iso)
    zh_text = (DOC_DIR / BASE_FILE).read_text(encoding="utf-8")
    tgt_text = target_file.read_text(encoding="utf-8")

    zh_segs = split_by_headings(zh_text)
    tgt_segs = split_by_headings(tgt_text)

    print(f"\n{'='*60}")
    print(f"  [{iso}] {name}  — {target_file.name}")
    print(f"  zh segments: {len(zh_segs)}  tgt segments: {len(tgt_segs)}")
    print(f"{'='*60}")

    results = []
    n = max(len(zh_segs), len(tgt_segs))

    def do_one(i):
        zh_s = zh_segs[i] if i < len(zh_segs) else (0, 0, "")
        tgt_s = tgt_segs[i] if i < len(tgt_segs) else (0, 0, "")
        zh_start, zh_end, zh_content = zh_s
        tgt_start, tgt_end, tgt_content = tgt_s

        heading_zh = zh_content.split("\n")[0].strip() if zh_content else "N/A"
        heading_tgt = tgt_content.split("\n")[0].strip() if tgt_content else "N/A"

        raw = call_llm(zh_content, tgt_content, iso)
        llm_ok, llm_raw = parse_llm(raw, f"{iso}[{i}]")

        verify = verify_segment(zh_content, tgt_content, llm_ok)

        return {
            "seg_idx": i,
            "zh_range": f"L{zh_start}-L{zh_end}",
            "tgt_range": f"L{tgt_start}-L{tgt_end}",
            "zh_heading": heading_zh,
            "tgt_heading": heading_tgt,
            "llm_semantic": llm_ok,
            "llm_raw": llm_raw,
            **verify,
        }

    with ThreadPoolExecutor(max_workers=MAX_CONCUR) as pool:
        futures = {pool.submit(do_one, i): i for i in range(n)}
        for fut in as_completed(futures):
            r = fut.result()
            results.append(r)
            ok = r["line_verdict"] == "OK" and r["struct_match"] and r["llm_semantic"] is not False
            if not ok:
                tags = []
                if r["line_verdict"] != "OK": tags.append(f"line:{r['line_verdict']}")
                if not r["struct_match"]: tags.append("struct_diff")
                if r["llm_semantic"] is False: tags.append("semantic_diff")
                if r["llm_semantic"] is None: tags.append("LLM_parse_fail")
                print(f"  seg[{r['seg_idx']:03d}] FAIL | {r['zh_heading'][:60]} | {' '.join(tags)}")

    results.sort(key=lambda x: x["seg_idx"])
    fail_count = sum(1 for r in results if not (r["line_verdict"] == "OK" and r["struct_match"] and r["llm_semantic"] is not False))
    print(f"  >>> {len(results)} segments, {len(results) - fail_count} OK, {fail_count} FAIL")
    return iso, name, results

# ── report ──────────────────────────────────────────────────
def write_report(all_results: list):
    """all_results: [(iso, name, [seg_results]), ...]"""
    out_path = BASE_DIR / "temp" / "_compare_report.md"
    lines = []
    lines.append("# 多语种技术文档对比报告")
    lines.append(f"生成时间: {time.strftime('%Y-%m-%d %H:%M:%S')}")
    lines.append(f"基准: {BASE_FILE}")
    lines.append("")

    total_ok = 0
    total_fail = 0

    for iso, name, segs in all_results:
        lines.append(f"## {iso} — {name}")
        lines.append("")
        ok = sum(1 for s in segs if s["line_verdict"] == "OK" and s["struct_match"] and s["llm_semantic"] is not False)
        fail = len(segs) - ok
        total_ok += ok
        total_fail += fail
        lines.append(f"**通过: {ok} / 失败: {fail} / 总计: {len(segs)}**")
        lines.append("")

        for s in segs:
            if not (s["line_verdict"] == "OK" and s["struct_match"] and s["llm_semantic"] is not False):
                lines.append(f"### seg[{s['seg_idx']:03d}] ❌")
                lines.append(f"- zh: `{s['zh_range']}` — `{s['zh_heading']}`")
                lines.append(f"- tgt: `{s['tgt_range']}` — `{s['tgt_heading']}`")
                lines.append(f"- 行数: zh={s['zh_lines']} tgt={s['tgt_lines']} match={s['line_match']}")
                lines.append(f"- LLM语义: `{s['llm_semantic']}` reason=`{s['llm_raw'][:200]}`")
                if s["struct_diffs"]:
                    lines.append(f"- 结构差异:")
                    for d in s["struct_diffs"]:
                        lines.append(f"  - {d}")
                lines.append("")

        lines.append(f"---")
        lines.append("")

    lines.insert(4, f"**总通过: {total_ok} / 总失败: {total_fail}**")
    lines.insert(5, "")

    out_path.write_text("\n".join(lines), encoding="utf-8")
    print(f"\n报告已写入: {out_path}")

# ── main ────────────────────────────────────────────────────
def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--dry-run", action="store_true", help="只切分不调LLM")
    parser.add_argument("--lang", type=str, default="", help="逗号分隔的iso码, 默认全部")
    parser.add_argument("--from", type=str, default="", dest="from_iso", help="起始iso (含)")
    parser.add_argument("--to", type=str, default="", dest="to_iso", help="结束iso (含)")
    args = parser.parse_args()

    all_md = sorted(DOC_DIR.glob("technical_reference_*.md"))
    targets = [f for f in all_md if f.name != BASE_FILE]

    if args.lang:
        wanted = set(args.lang.split(","))
        targets = [f for f in targets if iso_from_filename(f.name) in wanted]

    if args.from_iso or args.to_iso:
        from_i = next((i for i, f in enumerate(targets) if iso_from_filename(f.name) == args.from_iso), 0) if args.from_iso else 0
        to_i = next((i for i, f in enumerate(targets) if iso_from_filename(f.name) == args.to_iso), len(targets)-1) if args.to_iso else len(targets)-1
        targets = targets[from_i:to_i+1]

    print(f"基准: {BASE_FILE}")
    print(f"目标: {len(targets)} 个语种")
    for t in targets:
        print(f"  - {t.name}")

    if args.dry_run:
        print("\n[Dry-run] 只展示切分结果\n")
        zh_text = (DOC_DIR / BASE_FILE).read_text(encoding="utf-8")
        zh_segs = split_by_headings(zh_text)
        print(f"zh-hans 切分为 {len(zh_segs)} 段:")
        for i, (s, e, txt) in enumerate(zh_segs):
            h = txt.split("\n")[0][:70]
            print(f"  [{i:03d}] L{s}-L{e} | {h}")
        for tf in targets:
            iso = iso_from_filename(tf.name)
            txt = tf.read_text(encoding="utf-8")
            segs = split_by_headings(txt)
            print(f"\n{iso} 切分为 {len(segs)} 段:")
            for i, (s, e, txt2) in enumerate(segs):
                h = txt2.split("\n")[0][:70]
                print(f"  [{i:03d}] L{s}-L{e} | {h}")
        return

    all_results = []
    for tf in targets:
        iso, name, segs = process_lang(tf)
        all_results.append((iso, name, segs))

    write_report(all_results)

if __name__ == "__main__":
    main()

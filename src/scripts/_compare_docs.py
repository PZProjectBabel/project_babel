#!/usr/bin/env python3
"""
src/scripts/_compare_docs.py — 自动切分+LLM对比+二次核对 多文档多语种差异
支持文档家族: technical_reference, readme, contributing
用法: python src/scripts/_compare_docs.py [--dry-run] [--lang hu,ja,ko] [--family technical_reference,readme]
"""

import re, json, os, sys, time, argparse
from pathlib import Path
from concurrent.futures import ThreadPoolExecutor, as_completed
import requests

if sys.stdout.encoding != 'utf-8':
    sys.stdout.reconfigure(encoding='utf-8', errors='replace')

# ── config ──────────────────────────────────────────────────
BASE_DIR    = Path(__file__).resolve().parent.parent.parent  # project_babel root
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
        "base_path": BASE_DIR / "README.md",
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
SECRETS     = json.loads((BASE_DIR / "config" / "secrets.json").read_text(encoding="utf-8"))
CONFIG      = json.loads((BASE_DIR / "config" / "config.json").read_text(encoding="utf-8"))

LLM_KEY      = SECRETS["LLM_KEY"]
LLM_ENDPOINT = CONFIG["LLM"]["api_endpoint"]
LLM_MODEL    = "deepseek-v4-flash"
LLM_TIMEOUT  = 300
MAX_CONCUR   = 256
MAX_RETRIES  = 3

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
def iso_from_filename(fname: str, prefix: str) -> str:
    """去掉前缀和扩展名得到iso码"""
    return fname.replace(prefix, "").replace(".md", "")

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

# ── file-level structural pre-check ────────────────────────
def check_structures(fam: dict, targets: list) -> tuple:
    """
    检查所有目标文件与 zh-hans 的段落数/标题是否一致.
    返回 (all_ok: bool, issues: list[str])
    """
    base_file = fam.get("base_path", fam["dir"] / fam["base"])
    zh_segs = split_by_headings(base_file.read_text(encoding="utf-8"))
    issues = []
    for tf in targets:
        iso = iso_from_filename(tf.name, fam["prefix"])
        tgt_segs = split_by_headings(tf.read_text(encoding="utf-8"))
        if len(zh_segs) != len(tgt_segs):
            issues.append(
                f"[{iso}] 段落数不一致: zh={len(zh_segs)} tgt={len(tgt_segs)}"
            )
        n = min(len(zh_segs), len(tgt_segs))
        for i in range(n):
            # 只比较标题级别(#数量), 不比较翻译文本
            zh_h = zh_segs[i][2].split("\n")[0].strip()
            tgt_h = tgt_segs[i][2].split("\n")[0].strip()
            zh_lvl = len(zh_h) - len(zh_h.lstrip('#'))
            tgt_lvl = len(tgt_h) - len(tgt_h.lstrip('#'))
            if zh_lvl != tgt_lvl:
                issues.append(
                    f"[{iso}] seg[{i:03d}] 标题级别不一致 | "
                    f"zh(L{zh_lvl}): {zh_h[:60]} | tgt(L{tgt_lvl}): {tgt_h[:60]}"
                )
    return len(issues) == 0, issues

# ── LLM call ────────────────────────────────────────────────
PROMPT_TMPL = """\
你是语义对比机。只输出两行：
第一行: true 或 false (全部通过=true, 否则=false)
第二行: 简短原因(一行, 多问题用;分隔, 指出具体位置)

检查项(任一不通过则false):
A. 整体语义是否与中文原文一致
B. 目标语言段落中是否有未翻译的其它语言残留 (注意区分: 代码块内容/变量名/函数名/类名/文件名/路径/URL/Steam ID/专有名词/API字段名 不算残留)

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

def call_llm_with_retry(zh_text: str, tgt_text: str, target_iso: str) -> str:
    """调用LLM，失败最多重试MAX_RETRIES次"""
    for attempt in range(MAX_RETRIES):
        raw = call_llm(zh_text, tgt_text, target_iso)
        if not raw.startswith("ERROR:"):
            return raw
        if attempt < MAX_RETRIES - 1:
            time.sleep(1)
    return raw

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

# ── prepare segments per lang (不含LLM调用) ──────────────────
def prepare_lang_segments(target_file: Path, family: dict) -> tuple:
    """读取并切分文件，返回 (iso, name, list[seg_dict])"""
    iso = iso_from_filename(target_file.name, family["prefix"])
    name = lang_name(iso)
    base_file = family.get("base_path", family["dir"] / family["base"])
    zh_text = base_file.read_text(encoding="utf-8")
    tgt_text = target_file.read_text(encoding="utf-8")

    zh_segs = split_by_headings(zh_text)
    tgt_segs = split_by_headings(tgt_text)
    n = max(len(zh_segs), len(tgt_segs))

    segments = []
    for i in range(n):
        zh_s = zh_segs[i] if i < len(zh_segs) else (0, 0, "")
        tgt_s = tgt_segs[i] if i < len(tgt_segs) else (0, 0, "")
        zh_start, zh_end, zh_content = zh_s
        tgt_start, tgt_end, tgt_content = tgt_s
        heading_zh = zh_content.split("\n")[0].strip() if zh_content else "N/A"
        segments.append({
            "seg_idx": i,
            "iso": iso,
            "name": name,
            "zh_range": f"L{zh_start}-L{zh_end}",
            "tgt_range": f"L{tgt_start}-L{tgt_end}",
            "zh_heading": heading_zh,
            "zh_content": zh_content,
            "tgt_content": tgt_content,
        })
    return iso, name, segments

# ── process one segment task ────────────────────────────────
def process_one_task(seg: dict) -> dict:
    """单个分片: LLM调用(含重试)+校验，返回合并结果"""
    raw = call_llm_with_retry(seg["zh_content"], seg["tgt_content"], seg["iso"])
    llm_ok, llm_raw = parse_llm(raw, f"{seg['iso']}[{seg['seg_idx']}]")
    verify = verify_segment(seg["zh_content"], seg["tgt_content"], llm_ok)
    return {**seg, "llm_semantic": llm_ok, "llm_raw": llm_raw, **verify}

# ── report ──────────────────────────────────────────────────
def write_report(all_results: list, family_label: str, base_file: str):
    """all_results: [(iso, name, struct_issues, semantic_issues), ...]"""
    out_path = BASE_DIR / "temp" / "_compare_report.md"
    lines = []
    lines.append("# 多语种文档对比报告")
    lines.append(f"生成时间: {time.strftime('%Y-%m-%d %H:%M:%S')}")
    lines.append(f"文档家族: {family_label}")
    lines.append(f"基准: {base_file}")
    lines.append("")

    total_struct = 0
    total_semantic = 0

    # ── 结构问题 ──
    lines.append("## 结构不一致")
    lines.append("")
    for iso, name, struct_issues, _semantic_issues in all_results:
        if struct_issues:
            lines.append(f"### {iso} — {name} ({len(struct_issues)} 段)")
            lines.append("")
            for s in struct_issues:
                lines.append(f"- seg[{s['seg_idx']:03d}] `{s['zh_heading'][:60]}`")
                lines.append(f"  - 行数: zh={s['zh_lines']} tgt={s['tgt_lines']} match={s['line_match']}")
                if s["struct_diffs"]:
                    for d in s["struct_diffs"]:
                        lines.append(f"  - {d}")
                lines.append("")
            total_struct += len(struct_issues)

    # ── 语义问题 ──
    lines.append("## 语义不一致")
    lines.append("")
    for iso, name, _struct_issues, semantic_issues in all_results:
        if semantic_issues:
            lines.append(f"### {iso} — {name} ({len(semantic_issues)} 段)")
            lines.append("")
            for s in semantic_issues:
                lines.append(f"- seg[{s['seg_idx']:03d}] `{s['zh_heading'][:60]}`")
                lines.append(f"  - LLM: `{s['llm_semantic']}` reason=`{s['llm_raw'][:200]}`")
                lines.append("")
            total_semantic += len(semantic_issues)

    lines.insert(4, f"**结构问题: {total_struct} / 语义问题: {total_semantic}**")
    lines.insert(5, "")

    out_path.write_text("\n".join(lines), encoding="utf-8")
    print(f"\n报告: {out_path}")

# ── main ────────────────────────────────────────────────────
def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--dry-run", action="store_true", help="只切分不调LLM")
    parser.add_argument("--lang", type=str, default="", help="逗号分隔的iso码, 默认全部")
    parser.add_argument("--from", type=str, default="", dest="from_iso", help="起始iso (含)")
    parser.add_argument("--to", type=str, default="", dest="to_iso", help="结束iso (含)")
    parser.add_argument("--family", type=str, default="", help="逗号分隔的文档家族, 默认全部")
    args = parser.parse_args()

    families_to_run = args.family.split(",") if args.family else list(DOC_FAMILIES.keys())
    exit_code = 0

    for fam_name in families_to_run:
        if fam_name not in DOC_FAMILIES:
            print(f"[WARN] 未知文档家族: {fam_name}，跳过")
            continue
        fam = DOC_FAMILIES[fam_name]
        print(f"\n{'='*70}")
        print(f"  文档家族: {fam['label']} ({fam_name})")
        print(f"{'='*70}")

        all_md = sorted(fam["dir"].glob(fam["glob"]))
        targets = [f for f in all_md if f.name not in fam["skip"]]

        if args.lang:
            wanted = set(args.lang.split(","))
            targets = [f for f in targets if iso_from_filename(f.name, fam["prefix"]) in wanted]

        if args.from_iso or args.to_iso:
            names = [iso_from_filename(f.name, fam["prefix"]) for f in targets]
            from_i = next((i for i, n in enumerate(names) if n == args.from_iso), 0) if args.from_iso else 0
            to_i = next((i for i, n in enumerate(names) if n == args.to_iso), len(targets)-1) if args.to_iso else len(targets)-1
            targets = targets[from_i:to_i+1]

        print(f"基准: {fam['base']}")
        print(f"目标: {len(targets)} 个语种")

        if args.dry_run:
            print("\n[Dry-run] 段落切分预览\n")
            base_file = fam.get("base_path", fam["dir"] / fam["base"])
            zh_text = base_file.read_text(encoding="utf-8")
            zh_segs = split_by_headings(zh_text)
            print(f"{fam['base']} → {len(zh_segs)} 段:")
            for i, (s, e, txt) in enumerate(zh_segs):
                h = txt.split("\n")[0][:70]
                print(f"  [{i:03d}] L{s}-L{e} | {h}")
            for tf in targets:
                iso = iso_from_filename(tf.name, fam["prefix"])
                txt = tf.read_text(encoding="utf-8")
                segs = split_by_headings(txt)
                print(f"\n{iso} → {len(segs)} 段:")
                for i, (s, e, txt2) in enumerate(segs):
                    h = txt2.split("\n")[0][:70]
                    print(f"  [{i:03d}] L{s}-L{e} | {h}")
            continue

        # ── 阶段1: 文件级结构预检 ──
        struct_ok, file_struct_issues = check_structures(fam, targets)
        if not file_struct_issues:
            print("  文件结构: 全部一致 ✓")
        else:
            print(f"\n=== 文件结构不一致 ({len(file_struct_issues)} 项) — 跳过 LLM 比对 ===")
            for issue in file_struct_issues:
                print(f"  {issue}")
            exit_code = 1
            continue  # 跳过 LLM

        if not struct_ok:
            continue

        # ── 阶段2: 批量准备所有分片 ──
        all_segments = []
        for tf in targets:
            _iso, _name, segs = prepare_lang_segments(tf, fam)
            all_segments.extend(segs)

        print(f"  总任务数: {len(all_segments)} LLM调用 (并发={MAX_CONCUR}, 重试={MAX_RETRIES}次)")

        # ── 阶段3: 全量并行LLM调用(含重试) ──
        raw_results = []
        with ThreadPoolExecutor(max_workers=MAX_CONCUR) as pool:
            futures = {pool.submit(process_one_task, seg): seg for seg in all_segments}
            for fut in as_completed(futures):
                raw_results.append(fut.result())

        # 按iso分组并按seg_idx排序
        from collections import defaultdict
        by_iso = defaultdict(list)
        for r in raw_results:
            by_iso[r["iso"]].append(r)
        for iso in by_iso:
            by_iso[iso].sort(key=lambda x: x["seg_idx"])

        # ── 阶段4: 分类输出 ──
        all_results = []
        for tf in targets:
            iso = iso_from_filename(tf.name, fam["prefix"])
            name = lang_name(iso)
            seg_results = by_iso.get(iso, [])

            struct_issues = []
            semantic_issues = []
            for r in seg_results:
                is_struct = (r["line_verdict"] != "OK" or not r["struct_match"])
                is_semantic = (r["llm_semantic"] is False or r["llm_semantic"] is None)
                if is_struct:
                    struct_issues.append(r)
                elif is_semantic:
                    semantic_issues.append(r)

            all_results.append((iso, name, struct_issues, semantic_issues))

            if struct_issues:
                print(f"\n--- [{iso}] {name} 段落结构问题 ({len(struct_issues)} 段) ---")
                for r in struct_issues:
                    tags = []
                    if r["line_verdict"] != "OK":
                        tags.append(f"line:{r['line_verdict']}")
                    if not r["struct_match"]:
                        tags.append("struct_diff")
                    print(f"  seg[{r['seg_idx']:03d}] {r['zh_heading'][:60]} | {' '.join(tags)}")
                    exit_code = 1

            if semantic_issues:
                print(f"\n--- [{iso}] {name} 语义问题 ({len(semantic_issues)} 段) ---")
                for r in semantic_issues:
                    tag = "LLM_parse_fail" if r["llm_semantic"] is None else "semantic_diff"
                    print(f"  seg[{r['seg_idx']:03d}] {r['zh_heading'][:60]} | {tag} | {r['llm_raw'][:120]}")
                    exit_code = 1

        write_report(all_results, fam["label"], fam["base"])

    sys.exit(exit_code)

if __name__ == "__main__":
    main()

@echo off
chcp 65001 >nul
title Project Babel — 文档一致性检查
cd /d "%~dp0"

echo ============================================
echo  Project Babel — 文档一致性检查工具
echo ============================================
echo.

:: ── 1. venv ──
set PYTHON=.venv\Scripts\python.exe
if exist .venv\Scripts\activate.bat (
    call .venv\Scripts\activate.bat
) else (
    echo [WARN] 未找到 .venv，使用系统 Python
    set PYTHON=python
)
echo.

:: ── 2. 段落索引 ──
echo [1/4] 段落索引 (zh-hans 基准) ————————————
%PYTHON% src/scripts/_list_segments.py
if %errorlevel% neq 0 (
    echo [!] _list_segments.py 失败，请检查依赖
) else (
    echo [OK]
)
echo.

:: ── 3. CJK 残留扫描 ──
echo [2/4] CJK 残留扫描 ——————————————————————
echo  覆盖: technical_reference / readme / contributing
%PYTHON% src/scripts/_find_cjk.py
if %errorlevel% neq 0 (
    echo [!] _find_cjk.py 失败
) else (
    echo [OK]
)
echo.

:: ── 4. 多语言交叉连接检查 ──
echo [3/4] 交叉连接检查 ——————————————————————
echo  覆盖: technical_reference / readme / contributing
%PYTHON% src/scripts/_add_crosslinks.py
if %errorlevel% neq 0 (
    echo [!] _add_crosslinks.py 失败
) else (
    echo [OK]
)
echo.

:: ── 5. LLM 语义对比 (仅 dry-run) ──
echo [4/4] LLM 语义对比 (dry-run — 不调用 API) ——
echo  覆盖: technical_reference / readme / contributing
%PYTHON% src/scripts/_compare_docs.py --dry-run
if %errorlevel% neq 0 (
    echo [!] _compare_docs.py 失败
) else (
    echo [OK]
)
echo.

echo ============================================
echo  全部检查完成。
echo.
echo  覆盖文档: technical_reference / readme / contributing
echo.
echo  如需完整 LLM 对比（消耗 API 额度）:
echo    %PYTHON% src/scripts/_compare_docs.py
echo    python src/scripts/_compare_docs.py --family readme
echo    python src/scripts/_compare_docs.py --family contributing
echo.
echo  查看段落对应关系:
echo    python src/scripts/_list_segments.py docs/technical_reference/technical_reference_??.md
echo ============================================
pause

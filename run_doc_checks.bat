@echo off
chcp 65001 >nul
cd /d "%~dp0"

set PYTHON=.venv\Scripts\python.exe
if not exist ".venv\Scripts\python.exe" set PYTHON=python

%PYTHON% src/scripts/_doc_checks.py %*
pause

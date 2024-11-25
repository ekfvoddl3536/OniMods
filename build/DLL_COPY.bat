@ECHO OFF
setlocal EnableDelayedExpansion

title "DLL Auto-copy"

cd /d "%~dp0"

echo "Current Directory Info >>"
cd

:: Create 'bin' directory
if not exist "bin\" md "bin"

:: Read 'PATH.txt'
set /p DLL_PATH=<PATH.txt

:: Copy DLLs
for /f "tokens=*" %%i in (DLL_LIST.txt) do (
    copy "!DLL_PATH!\%%i" "bin\%%i"
)

echo "Done!"
timeout 5
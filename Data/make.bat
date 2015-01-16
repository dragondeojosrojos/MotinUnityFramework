call config.bat
@echo off
cls
echo *************************************************************************
echo                        BUILDING DATA
echo *

 
cd strings
call make.bat
rem %PYTHON% %TOOLS_DIR%\pyScripts\src\generateStringsNamesCsharp.py %DATA_DIR%#temp\strings\String_IDs.h %GENERATED_SRC%strings.cs

echo *************************************************************************
echo                        BUILD END
echo *************************************************************************

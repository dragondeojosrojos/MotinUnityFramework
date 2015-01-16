cd..
call config.bat
@echo off

cd %DATA_DIR%

rem Exporting XLS file
set XLS_STRINGS_FILE=./Strings/Strings.xls


python ./Strings/buildStringPack.py %XLS_STRINGS_FILE% ../Assets/Resources/strings/ ../Assets/MotinGames/StringManager/Generated/
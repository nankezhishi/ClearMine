hhc ClearMineHelp.hhp
set BASE_DIR=%~dp0
cp -u ClearMineHelp.chm -t %BASE_DIR%..\bin\debug
cp -u ClearMineHelp.chm -t %BASE_DIR%..\bin\release
pause
exit
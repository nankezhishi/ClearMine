!include "MUI2.nsh"
!include "uninstallfromlog.nsh"

Name "Clear Mine"
OutFile "ClearMineInstall.exe"
InstallDir "C:\Program Files\Poleaf Software\ClearMine"
RequestExecutionLevel user

!define MUI_ABORTWARNING
!define MUI_FINISHPAGE_NOREBOOTSUPPORT

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

!insertmacro MUI_LANGUAGE "SimpChinese"
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "TradChinese"

Section "-LogSetOn"
    LogSet on
SectionEnd

 Section "install"
    SetOutPath $INSTDIR
    WriteUninstaller $INSTDIR\Uninstaller.exe
    File ..\Bin\Release\*.dll
    File ..\Bin\Release\ClearMine.exe
    File ..\Bin\Release\ClearMine.exe.config
    File ..\Bin\Release\ClearMineHelp.chm
    SetOutPath $INSTDIR\Localization\zh-CN
    File ..\ClearMine.Localization\zh-CN\Overall.xaml
    SetOutPath $INSTDIR\Themes
    File ..\ClearMine.Themes\Themes\luna.normalcolor.xaml
    SetOutPath $INSTDIR\Sound
    File ..\Bin\Release\Sound\*.wma
SectionEnd

Section Uninstall
    SetAutoClose false
    Call un.CreateLogFromFile
    Call un.RemoveDirectoriesFromLog
    Delete $INSTDIR\install.log
    Delete $INSTDIR\Uninstaller.exe
    RMDir $INSTDIR
SectionEnd

Function .onInit
 !insertmacro MUI_LANGDLL_DISPLAY
FunctionEnd

Function un.onInit
 !insertmacro MUI_UNGETLANGUAGE
FunctionEnd
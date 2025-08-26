; StreamVault NSIS Installer Script
; Requires NSIS (Nullsoft Scriptable Install System)
; Download from: https://nsis.sourceforge.io/

;--------------------------------
; General

!define APP_NAME "StreamVault"
!define APP_VERSION "1.0.0"
!define APP_PUBLISHER "StreamVault Team"
!define APP_URL "https://github.com/streamvault"
!define APP_EXECUTABLE "StreamVault.exe"

; The name of the installer
Name "${APP_NAME} ${APP_VERSION}"

; The file to write
OutFile "StreamVault_Setup.exe"

; Request application privileges for Windows Vista/7/8/10/11
RequestExecutionLevel admin

; Build Unicode installer
Unicode True

; The default installation directory
InstallDir "$PROGRAMFILES\${APP_NAME}"

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\${APP_NAME}" "Install_Dir"

;--------------------------------
; Interface Settings

!include "MUI2.nsh"

!define MUI_ABORTWARNING
!define MUI_ICON "StreamVault.exe"
!define MUI_UNICON "StreamVault.exe"

;--------------------------------
; Pages

!insertmacro MUI_PAGE_LICENSE "LICENSE.txt"
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

;--------------------------------
; Languages

!insertmacro MUI_LANGUAGE "English"

;--------------------------------
; Installer Sections

Section "StreamVault (required)" SecMain
  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put files there
  File /r "*.*"
  
  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\${APP_NAME} "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "DisplayName" "${APP_NAME}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "DisplayVersion" "${APP_VERSION}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "Publisher" "${APP_PUBLISHER}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "URLInfoAbout" "${APP_URL}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "NoRepair" 1
  WriteUninstaller "$INSTDIR\uninstall.exe"
  
SectionEnd

Section "Desktop Shortcut" SecDesktop
  CreateShortcut "$DESKTOP\${APP_NAME}.lnk" "$INSTDIR\${APP_EXECUTABLE}" "" "$INSTDIR\${APP_EXECUTABLE}" 0
SectionEnd

Section "Start Menu Shortcuts" SecStartMenu
  CreateDirectory "$SMPROGRAMS\${APP_NAME}"
  CreateShortcut "$SMPROGRAMS\${APP_NAME}\${APP_NAME}.lnk" "$INSTDIR\${APP_EXECUTABLE}" "" "$INSTDIR\${APP_EXECUTABLE}" 0
  CreateShortcut "$SMPROGRAMS\${APP_NAME}\Uninstall.lnk" "$INSTDIR\uninstall.exe"
SectionEnd

Section "FFmpeg" SecFFmpeg
  DetailPrint "Checking for FFmpeg..."
  
  ; Check if FFmpeg is already in PATH
  ExecWait 'ffmpeg -version' $0
  ${If} $0 == 0
    DetailPrint "FFmpeg found in system PATH"
  ${Else}
    DetailPrint "FFmpeg not found, installing..."
    
    ; Create ffmpeg directory
    CreateDirectory "$INSTDIR\ffmpeg"
    CreateDirectory "$INSTDIR\ffmpeg\bin"
    
    ; Download FFmpeg (this is simplified - in reality you'd include ffmpeg.exe)
    DetailPrint "Downloading FFmpeg..."
    inetc::get "https://github.com/BtbN/FFmpeg-Builds/releases/download/latest/ffmpeg-master-latest-win64-gpl.zip" "$TEMP\ffmpeg.zip" /END
    Pop $0
    ${If} $0 == "OK"
      DetailPrint "Extracting FFmpeg..."
      nsisunz::UnzipToLog "$TEMP\ffmpeg.zip" "$INSTDIR\ffmpeg"
      Delete "$TEMP\ffmpeg.zip"
    ${Else}
      DetailPrint "FFmpeg download failed. StreamVault will attempt to download it when first run."
    ${EndIf}
  ${EndIf}
SectionEnd

;--------------------------------
; Descriptions

LangString DESC_SecMain ${LANG_ENGLISH} "Install the main StreamVault application."
LangString DESC_SecDesktop ${LANG_ENGLISH} "Create a desktop shortcut for StreamVault."
LangString DESC_SecStartMenu ${LANG_ENGLISH} "Create Start Menu shortcuts for StreamVault."
LangString DESC_SecFFmpeg ${LANG_ENGLISH} "Download and install FFmpeg for video streaming capabilities."

!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${SecMain} $(DESC_SecMain)
  !insertmacro MUI_DESCRIPTION_TEXT ${SecDesktop} $(DESC_SecDesktop)
  !insertmacro MUI_DESCRIPTION_TEXT ${SecStartMenu} $(DESC_SecStartMenu)
  !insertmacro MUI_DESCRIPTION_TEXT ${SecFFmpeg} $(DESC_SecFFmpeg)
!insertmacro MUI_FUNCTION_DESCRIPTION_END

;--------------------------------
; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}"
  DeleteRegKey HKLM SOFTWARE\${APP_NAME}

  ; Remove files and uninstaller
  Delete "$INSTDIR\*.*"
  RMDir /r "$INSTDIR"

  ; Remove shortcuts, if any
  Delete "$DESKTOP\${APP_NAME}.lnk"
  Delete "$SMPROGRAMS\${APP_NAME}\*.*"
  RMDir "$SMPROGRAMS\${APP_NAME}"

SectionEnd

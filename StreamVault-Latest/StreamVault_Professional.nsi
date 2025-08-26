; StreamVault Professional Installer
; Nullsoft Scriptable Install System (NSIS) Script
; Creates a professional Windows installer EXE

;--------------------------------
; Installer Configuration
!define APP_NAME "StreamVault"
!define APP_VERSION "1.0.0"
!define APP_PUBLISHER "StreamVault Team"
!define APP_URL "https://github.com/streamvault"
!define APP_DESCRIPTION "Professional Multi-Monitor SRT Streaming Solution"

; Installer file name
!define INSTALLER_NAME "StreamVault_Setup_v${APP_VERSION}.exe"

; Installation directory
!define INSTALL_DIR "$PROGRAMFILES\${APP_NAME}"

; Registry keys
!define REG_UNINSTALL "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}"
!define REG_APP "SOFTWARE\${APP_NAME}"

;--------------------------------
; Include Modern UI
!include "MUI2.nsh"
!include "WinVer.nsh"
!include "x64.nsh"
!include "FileFunc.nsh"
!include "LogicLib.nsh"

;--------------------------------
; General Settings
Name "${APP_NAME} ${APP_VERSION}"
OutFile "${INSTALLER_NAME}"
InstallDir "${INSTALL_DIR}"
InstallDirRegKey HKLM "${REG_APP}" "InstallLocation"
RequestExecutionLevel admin
ShowInstDetails show
ShowUnInstDetails show

; Compression
SetCompressor /solid lzma
SetCompressorDictSize 32

; Version Information
VIProductVersion "1.0.0.0"
VIAddVersionKey "ProductName" "${APP_NAME}"
VIAddVersionKey "ProductVersion" "${APP_VERSION}"
VIAddVersionKey "CompanyName" "${APP_PUBLISHER}"
VIAddVersionKey "FileDescription" "${APP_DESCRIPTION}"
VIAddVersionKey "FileVersion" "${APP_VERSION}"
VIAddVersionKey "LegalCopyright" "© 2025 ${APP_PUBLISHER}"

;--------------------------------
; Interface Settings
!define MUI_ABORTWARNING
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\modern-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; Header image
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_RIGHT
!define MUI_HEADERIMAGE_BITMAP "${NSISDIR}\Contrib\Graphics\Header\nsis3-metro.bmp"
!define MUI_HEADERIMAGE_UNBITMAP "${NSISDIR}\Contrib\Graphics\Header\nsis3-metro.bmp"

; Welcome page
!define MUI_WELCOMEPAGE_TITLE "Welcome to ${APP_NAME} Setup"
!define MUI_WELCOMEPAGE_TEXT "This wizard will guide you through the installation of ${APP_NAME}.$\r$\n$\r$\n${APP_DESCRIPTION}$\r$\n$\r$\nClick Next to continue."

; Finish page
!define MUI_FINISHPAGE_RUN "$INSTDIR\StreamVault.exe"
!define MUI_FINISHPAGE_RUN_TEXT "Launch ${APP_NAME}"
!define MUI_FINISHPAGE_LINK "Visit ${APP_NAME} website"
!define MUI_FINISHPAGE_LINK_LOCATION "${APP_URL}"

;--------------------------------
; Pages
!insertmacro MUI_PAGE_WELCOME
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
!insertmacro MUI_LANGUAGE "Italian"

;--------------------------------
; License
LicenseData "LICENSE.txt"

;--------------------------------
; Installer Sections

Section "!${APP_NAME} Core" SecCore
    SectionIn RO  ; Required section
    
    DetailPrint "Installing ${APP_NAME} core files..."
    
    ; Set output path
    SetOutPath "$INSTDIR"
    
    ; Copy all application files from app directory
    File /r "app\*.*"
    
    ; Create uninstaller
    WriteUninstaller "$INSTDIR\Uninstall.exe"
    
    ; Registry entries for uninstaller
    WriteRegStr HKLM "${REG_UNINSTALL}" "DisplayName" "${APP_NAME}"
    WriteRegStr HKLM "${REG_UNINSTALL}" "DisplayVersion" "${APP_VERSION}"
    WriteRegStr HKLM "${REG_UNINSTALL}" "Publisher" "${APP_PUBLISHER}"
    WriteRegStr HKLM "${REG_UNINSTALL}" "URLInfoAbout" "${APP_URL}"
    WriteRegStr HKLM "${REG_UNINSTALL}" "DisplayIcon" "$INSTDIR\StreamVault.exe,0"
    WriteRegStr HKLM "${REG_UNINSTALL}" "UninstallString" "$INSTDIR\Uninstall.exe"
    WriteRegStr HKLM "${REG_UNINSTALL}" "QuietUninstallString" "$INSTDIR\Uninstall.exe /S"
    WriteRegDWORD HKLM "${REG_UNINSTALL}" "NoModify" 1
    WriteRegDWORD HKLM "${REG_UNINSTALL}" "NoRepair" 1
    
    ; Application registry entries
    WriteRegStr HKLM "${REG_APP}" "InstallLocation" "$INSTDIR"
    WriteRegStr HKLM "${REG_APP}" "Version" "${APP_VERSION}"
    
    ; Calculate installed size
    ${GetSize} "$INSTDIR" "/S=0K" $0 $1 $2
    IntFmt $0 "0x%08X" $0
    WriteRegDWORD HKLM "${REG_UNINSTALL}" "EstimatedSize" "$0"
    
SectionEnd

Section "FFmpeg Integration" SecFFmpeg
    DetailPrint "Downloading and installing FFmpeg..."
    
    ; Create ffmpeg directory
    CreateDirectory "$INSTDIR\ffmpeg"
    SetOutPath "$INSTDIR\ffmpeg"
    
    ; Download FFmpeg
    DetailPrint "Downloading FFmpeg (this may take a few minutes)..."
    
    ; Use PowerShell to download FFmpeg
    nsExec::ExecToStack 'powershell -Command "& {[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; Invoke-WebRequest -Uri ''https://github.com/BtbN/FFmpeg-Builds/releases/download/latest/ffmpeg-master-latest-win64-gpl.zip'' -OutFile ''$TEMP\ffmpeg.zip''}"'
    Pop $0
    
    ${If} $0 == 0
        DetailPrint "FFmpeg downloaded successfully"
        
        ; Extract FFmpeg
        DetailPrint "Extracting FFmpeg..."
        nsExec::ExecToStack 'powershell -Command "Expand-Archive -Path ''$ENV:TEMP\ffmpeg.zip'' -DestinationPath ''$ENV:TEMP\ffmpeg_extract'' -Force"'
        Pop $0
        
        ${If} $0 == 0
            ; Find and copy ffmpeg.exe and ffprobe.exe
            nsExec::ExecToStack 'powershell -Command "Get-ChildItem -Path ''$ENV:TEMP\ffmpeg_extract'' -Recurse -Name ''ffmpeg.exe'' | ForEach-Object { Copy-Item ''$ENV:TEMP\ffmpeg_extract\$_'' ''$INSTDIR\ffmpeg\ffmpeg.exe'' }"'
            nsExec::ExecToStack 'powershell -Command "Get-ChildItem -Path ''$ENV:TEMP\ffmpeg_extract'' -Recurse -Name ''ffprobe.exe'' | ForEach-Object { Copy-Item ''$ENV:TEMP\ffmpeg_extract\$_'' ''$INSTDIR\ffmpeg\ffprobe.exe'' }"'
            
            ; Clean up temporary files
            Delete "$TEMP\ffmpeg.zip"
            RMDir /r "$TEMP\ffmpeg_extract"
            
            DetailPrint "FFmpeg installed successfully"
        ${Else}
            DetailPrint "Failed to extract FFmpeg - will be downloaded on first run"
        ${EndIf}
    ${Else}
        DetailPrint "Failed to download FFmpeg - will be downloaded on first run"
    ${EndIf}
    
SectionEnd

Section "Desktop Shortcut" SecDesktop
    DetailPrint "Creating desktop shortcut..."
    CreateShortCut "$DESKTOP\${APP_NAME}.lnk" "$INSTDIR\StreamVault.exe" \
        "" "$INSTDIR\StreamVault.exe" 0 SW_SHOWNORMAL \
        "" "${APP_DESCRIPTION}"
SectionEnd

Section "Start Menu Shortcuts" SecStartMenu
    DetailPrint "Creating Start Menu shortcuts..."
    CreateDirectory "$SMPROGRAMS\${APP_NAME}"
    CreateShortCut "$SMPROGRAMS\${APP_NAME}\${APP_NAME}.lnk" "$INSTDIR\StreamVault.exe" \
        "" "$INSTDIR\StreamVault.exe" 0 SW_SHOWNORMAL \
        "" "${APP_DESCRIPTION}"
    CreateShortCut "$SMPROGRAMS\${APP_NAME}\Uninstall.lnk" "$INSTDIR\Uninstall.exe" \
        "" "$INSTDIR\Uninstall.exe" 0 SW_SHOWNORMAL \
        "" "Uninstall ${APP_NAME}"
SectionEnd

;--------------------------------
; Section Descriptions

!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SecCore} "${APP_NAME} core application and runtime files. This component is required."
    !insertmacro MUI_DESCRIPTION_TEXT ${SecFFmpeg} "Download and install FFmpeg for video processing. Recommended for offline installation."
    !insertmacro MUI_DESCRIPTION_TEXT ${SecDesktop} "Create a desktop shortcut for easy access to ${APP_NAME}."
    !insertmacro MUI_DESCRIPTION_TEXT ${SecStartMenu} "Create Start Menu shortcuts for ${APP_NAME}."
!insertmacro MUI_FUNCTION_DESCRIPTION_END

;--------------------------------
; Functions

Function .onInit
    ; Check Windows version
    ${IfNot} ${AtLeastWin10}
        MessageBox MB_OK|MB_ICONSTOP "This application requires Windows 10 or later."
        Quit
    ${EndIf}
    
    ; Check if 64-bit
    ${IfNot} ${RunningX64}
        MessageBox MB_OK|MB_ICONSTOP "This application requires a 64-bit version of Windows."
        Quit
    ${EndIf}
    
    ; Check if already installed
    ReadRegStr $R0 HKLM "${REG_UNINSTALL}" "UninstallString"
    StrCmp $R0 "" done
    
    MessageBox MB_OKCANCEL|MB_ICONEXCLAMATION \
        "${APP_NAME} is already installed. $\n$\nClick OK to remove the \
        previous version or Cancel to cancel this upgrade." \
        IDOK uninst
    Abort
    
    uninst:
        ClearErrors
        ExecWait '$R0 /S _?=$INSTDIR'
        
        IfErrors no_remove_uninstaller done
        
        IfFileExists "$INSTDIR\StreamVault.exe" 0 no_remove_uninstaller
        
        MessageBox MB_OK|MB_ICONSTOP \
            "Previous installation could not be removed."
        Abort
        
    no_remove_uninstaller:
    
    done:
FunctionEnd

;--------------------------------
; Uninstaller

Section "Uninstall"
    DetailPrint "Removing ${APP_NAME}..."
    
    ; Remove registry keys
    DeleteRegKey HKLM "${REG_UNINSTALL}"
    DeleteRegKey HKLM "${REG_APP}"
    
    ; Remove shortcuts
    Delete "$DESKTOP\${APP_NAME}.lnk"
    Delete "$SMPROGRAMS\${APP_NAME}\*.*"
    RMDir "$SMPROGRAMS\${APP_NAME}"
    
    ; Remove application files
    RMDir /r "$INSTDIR\ffmpeg"
    Delete "$INSTDIR\*.*"
    
    ; Remove application directories
    RMDir "$INSTDIR"
    
    DetailPrint "${APP_NAME} has been successfully removed."
    
SectionEnd

;--------------------------------
; Installer Functions

Function .onInstSuccess
    MessageBox MB_YESNO "${APP_NAME} has been installed successfully!$\r$\n$\r$\nWould you like to view the README file?" IDNO +2
    ExecShell "open" "$INSTDIR\README.md"
FunctionEnd

Function un.onInit
    MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 \
        "Are you sure you want to completely remove ${APP_NAME} and all of its components?" \
        IDYES +2
    Abort
FunctionEnd

Function un.onUninstSuccess
    HideWindow
    MessageBox MB_ICONINFORMATION|MB_OK \
        "${APP_NAME} was successfully removed from your computer."
FunctionEnd

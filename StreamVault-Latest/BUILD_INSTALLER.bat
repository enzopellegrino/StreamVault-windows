@echo off
echo ==============================================
echo StreamVault v1.3.0 Installer Builder
echo ==============================================
echo.

:: Check if NSIS is installed
where makensis >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: NSIS (Nullsoft Scriptable Install System) not found!
    echo Please install NSIS from: https://nsis.sourceforge.io/Download
    echo.
    pause
    exit /b 1
)

:: Change to the script directory
cd /d "%~dp0"

:: Verify required files exist
if not exist "app\StreamVault.exe" (
    echo ERROR: StreamVault.exe not found in app directory!
    echo Please build the application first using: dotnet publish
    echo.
    pause
    exit /b 1
)

if not exist "..\drivers\IddSampleDriver.inf" (
    echo ERROR: Virtual desktop drivers not found!
    echo Please ensure drivers directory exists with IddSampleDriver.inf
    echo.
    pause
    exit /b 1
)

echo Building StreamVault Professional Setup v1.3.0...
echo.

:: Compile the NSIS installer
makensis /V3 StreamVault_Professional.nsi

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ==============================================
    echo ✅ SUCCESS: Installer created successfully!
    echo ==============================================
    echo File: StreamVault_Professional_Setup_v1.3.0.exe
    echo Size: 
    for %%A in (StreamVault_Professional_Setup_v1.3.0.exe) do echo   %%~zA bytes
    echo.
    echo The installer includes:
    echo • StreamVault v1.3.0 application
    echo • Virtual Desktop Drivers (IddSampleDriver)
    echo • FFmpeg integration (downloaded during install)
    echo • Desktop and Start Menu shortcuts
    echo • Professional uninstaller
    echo.
    
    :: Check if installer was created
    if exist "StreamVault_Professional_Setup_v1.3.0.exe" (
        echo Do you want to test the installer now? (Y/N)
        choice /c YN /n
        if !ERRORLEVEL! EQU 1 (
            echo.
            echo Starting installer in test mode...
            start "" "StreamVault_Professional_Setup_v1.3.0.exe"
        )
    )
) else (
    echo.
    echo ==============================================
    echo ❌ ERROR: Failed to create installer!
    echo ==============================================
    echo Please check the NSIS output above for errors.
    echo Common issues:
    echo • Missing files in app directory
    echo • Missing drivers directory
    echo • Incorrect file paths in .nsi script
    echo • NSIS syntax errors
)

echo.
pause

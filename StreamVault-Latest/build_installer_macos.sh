#!/bin/bash

echo "=============================================="
echo "StreamVault v1.4.0 Installer Builder"
echo "=============================================="
echo ""

# Check if NSIS is available
if ! command -v makensis &> /dev/null; then
    echo "❌ ERROR: NSIS (makensis) not found!"
    echo "Please install NSIS:"
    echo "• macOS: brew install nsis"
    echo "• Ubuntu: apt-get install nsis"
    echo "• Or download from: https://nsis.sourceforge.io/Download"
    exit 1
fi

echo "✅ NSIS found: $(makensis -VERSION)"
echo ""

# Check if app directory exists
if [ ! -d "app" ]; then
    echo "❌ ERROR: app directory not found!"
    echo "Please ensure the application is built and published first."
    exit 1
fi

# Check if main executable exists
if [ ! -f "app/StreamVault.exe" ]; then
    echo "❌ ERROR: StreamVault.exe not found in app directory!"
    echo "Please build the application first using: dotnet publish"
    exit 1
fi

# Check if drivers exist
if [ ! -f "../drivers/IddSampleDriver.inf" ]; then
    echo "❌ ERROR: Virtual desktop drivers not found!"
    echo "Please ensure drivers directory exists with IddSampleDriver.inf"
    exit 1
fi

echo "✅ Pre-flight checks passed!"
echo ""

# Build the actual installer using NSIS
INSTALLER_NAME="StreamVault_Professional_Setup_v1.3.0.exe"
APP_SIZE=$(du -sh app | cut -f1)
DRIVERS_SIZE=$(du -sh ../drivers | cut -f1)

echo "Building installer with the following components:"
echo "• StreamVault v1.4.0 application (161M)"
echo "• Virtual Desktop Drivers ( 16K)"
echo "• FFmpeg integration (downloaded during install)"
echo "• Desktop and Start Menu shortcuts"
echo "• Professional uninstaller"
echo ""

echo "Compiling NSIS installer..."
echo "Running: makensis StreamVault_Professional.nsi"
echo ""

# Compile the NSIS installer
if makensis StreamVault_Professional.nsi; then
    echo ""
    echo "=============================================="
    echo "✅ SUCCESS: Installer created successfully!"
    echo "=============================================="
    
    if [ -f "$INSTALLER_NAME" ]; then
        INSTALLER_SIZE=$(du -sh "$INSTALLER_NAME" | cut -f1)
        echo "File: $INSTALLER_NAME"
        echo "Size: $INSTALLER_SIZE"
        echo ""
        echo "The installer includes:"
        echo "• StreamVault v1.4.0 with virtual desktop support"
        echo "• IddSampleDriver for virtual display creation"
        echo "• Automatic FFmpeg download and setup"
        echo "• Professional Windows integration"
        echo "• Complete uninstaller with cleanup"
        echo ""
        echo "✅ Ready for distribution on Windows!"
    else
        echo "⚠️  Installer compilation completed but file not found."
        echo "Expected: $INSTALLER_NAME"
    fi
else
    echo ""
    echo "=============================================="
    echo "❌ ERROR: Failed to create installer!"
    echo "=============================================="
    echo "Please check the NSIS output above for errors."
    echo "Common issues:"
    echo "• Missing files in app directory"
    echo "• Missing drivers directory"
    echo "• Incorrect file paths in .nsi script"
    echo "• NSIS syntax errors"
    exit 1
fi
echo ""

echo "=============================================="
echo "✅ StreamVault v1.4.0 installer build complete!"
echo "=============================================="

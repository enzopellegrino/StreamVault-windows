#!/bin/bash

# StreamVault v1.4.1 GitHub Release Creation Script
# This script creates a GitHub release with the installer files

echo "================================================"
echo "🚀 Creating StreamVault v1.4.1 GitHub Release"
echo "================================================"

# Release information
RELEASE_TAG="v1.4.1"
RELEASE_TITLE="StreamVault v1.4.1 - Enhanced UI & Simplified Startup"
RELEASE_BODY="$(cat GITHUB_RELEASE_NOTES_v1.4.1.md)"

# Files to upload
INSTALLER_FILE="StreamVault_Professional_Setup_v1.4.1.exe"
PORTABLE_FILE="StreamVault_Portable_v1.4.1.zip"

echo "📋 Release Details:"
echo "   Tag: $RELEASE_TAG"
echo "   Title: $RELEASE_TITLE"
echo "   Files: $INSTALLER_FILE, $PORTABLE_FILE"
echo ""

# Check if files exist
if [ ! -f "$INSTALLER_FILE" ]; then
    echo "❌ Error: $INSTALLER_FILE not found!"
    exit 1
fi

if [ ! -f "$PORTABLE_FILE" ]; then
    echo "❌ Error: $PORTABLE_FILE not found!"
    exit 1
fi

echo "✅ All files found!"
echo ""

# Check if GitHub CLI is available
if command -v gh &> /dev/null; then
    echo "🔧 GitHub CLI found! Creating release..."
    
    # Create release with GitHub CLI
    gh release create "$RELEASE_TAG" \
        "$INSTALLER_FILE" \
        "$PORTABLE_FILE" \
        --title "$RELEASE_TITLE" \
        --notes "$RELEASE_BODY" \
        --latest
    
    if [ $? -eq 0 ]; then
        echo "✅ Release created successfully!"
        echo "🌐 View at: https://github.com/enzopellegrino/StreamVault-windows/releases/tag/$RELEASE_TAG"
    else
        echo "❌ Error creating release with GitHub CLI"
        exit 1
    fi
else
    echo "⚠️  GitHub CLI not found!"
    echo "📝 Manual release creation required:"
    echo ""
    echo "1. Go to: https://github.com/enzopellegrino/StreamVault-windows/releases/new"
    echo "2. Choose tag: $RELEASE_TAG"
    echo "3. Release title: $RELEASE_TITLE"
    echo "4. Upload files:"
    echo "   - $INSTALLER_FILE"
    echo "   - $PORTABLE_FILE"
    echo "5. Copy release notes from GITHUB_RELEASE_NOTES_v1.4.0.md"
    echo ""
    echo "📂 Files ready for upload:"
    ls -la "$INSTALLER_FILE" "$PORTABLE_FILE"
fi

echo ""
echo "================================================"
echo "🎉 StreamVault v1.4.1 Release Process Complete!"
echo "================================================"

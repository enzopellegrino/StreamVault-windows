# StreamVault Virtual Desktop Driver - Troubleshooting Guide

## ❌ Common Driver Installation Issues

### 1. Administrator Privileges Required
**Problem**: Driver installation fails with "Access Denied"
**Solution**: 
- Run StreamVault as Administrator (right-click → "Run as administrator")
- Ensure User Account Control (UAC) is enabled
- Check Windows security policies

### 2. Windows Test Mode Required
**Problem**: Driver installation fails with "Digital signature verification failed"
**Solution**:
```cmd
# Enable test mode (run as Administrator)
bcdedit /set testsigning on
# Restart computer
shutdown /r /t 0
```

### 3. Driver Verification Issues
**Problem**: Windows blocks unsigned drivers
**Solution**:
- Enable test signing mode (see above)
- Or use signed drivers from Microsoft Store/Hardware vendors
- Disable Driver Signature Enforcement temporarily

### 4. IddSampleDriver Not Found
**Problem**: Driver files missing or corrupted
**Solution**:
- Verify `drivers/IddSampleDriver.inf` exists
- Download complete package from GitHub releases
- Check file integrity

### 5. Device Manager Errors
**Problem**: Driver appears with yellow warning in Device Manager
**Solution**:
- Update driver through Device Manager
- Uninstall and reinstall driver
- Check Windows Update for compatible drivers

## 🔧 Step-by-Step Driver Installation

### Manual Installation Method
1. **Open Device Manager** (`devmgmt.msc`)
2. **Add Legacy Hardware** → Next → Install manually → Have Disk
3. **Browse** to StreamVault `drivers` folder
4. **Select** `IddSampleDriver.inf`
5. **Install** and restart if prompted

### Command Line Installation
```cmd
# Run as Administrator
pnputil /add-driver drivers\IddSampleDriver.inf /install
```

### PowerShell Method
```powershell
# Run as Administrator
Add-WindowsDriver -Online -Driver "drivers\IddSampleDriver.inf"
```

## 🛠️ Alternative Solutions

### Option 1: Use Microsoft Store Drivers
- Search for "Virtual Display" in Microsoft Store
- Install certified virtual display drivers
- Configure through StreamVault after installation

### Option 2: Spacedesk Integration
- Download Spacedesk from official website
- Install Spacedesk driver
- StreamVault will detect and use automatically

### Option 3: Third-Party Virtual Displays
- Virtual Display Manager
- DisplayLink software
- VNC Virtual Display drivers

## 🔍 Diagnostic Commands

### Check Driver Status
```cmd
# List all display drivers
pnputil /enum-drivers | findstr -i "display"

# Check virtual display devices
wmic path win32_videocontroller get name,status
```

### Event Log Analysis
1. Open **Event Viewer** (`eventvwr.msc`)
2. Navigate to **Windows Logs** → **System**
3. Filter by **Source**: "Microsoft-Windows-Kernel-PnP"
4. Look for driver installation errors

## 🚨 Known Limitations

### Windows Compatibility
- **Windows 11**: Full support with latest drivers
- **Windows 10 (20H2+)**: Supported with test mode
- **Windows 10 (older)**: May require additional configuration
- **Windows Server**: Limited support

### Hardware Requirements
- **Graphics Card**: DirectX 11+ compatible
- **RAM**: 8GB+ recommended for virtual displays
- **CPU**: Modern x64 processor

## 🆘 Emergency Workarounds

### If Driver Installation Completely Fails

#### Option A: Screen Capture Mode
```csharp
// StreamVault can still capture existing monitors
// Use "Multi Stream" without virtual desktop creation
// Capture physical monitors or extended displays
```

#### Option B: Software-Only Virtual Displays
```csharp
// Use Windows built-in virtual desktop features
// Configure multiple workspaces
// Stream workspace content instead of virtual hardware
```

#### Option C: Network Display Solutions
```csharp
// Use network-based virtual displays
// Configure second device as extended display
// Stream to network display via SRT
```

## 📞 Support Resources

### GitHub Issues Template
When reporting driver issues, include:
- Windows version (`winver`)
- Error messages from Event Viewer
- StreamVault debug log
- Output of: `pnputil /enum-drivers`

### Community Solutions
- Check GitHub Discussions for community fixes
- Search existing issues for similar problems
- Contribute solutions back to community

### Professional Support
- Consider hardware virtual display solutions
- Contact Microsoft for driver certification
- Use enterprise display management tools

---

## 🔄 Recovery Commands

### Remove Failed Driver Installation
```cmd
# Remove all IDD drivers
pnputil /delete-driver oem*.inf /uninstall

# Clean driver cache
pnputil /delete-driver oem*.inf /force

# Restart display service
net stop "Display Enhancement Service"
net start "Display Enhancement Service"
```

### Reset to Clean State
```cmd
# Disable test signing
bcdedit /set testsigning off

# Remove all virtual displays
# Restart computer
shutdown /r /t 0
```

---

**Remember**: Virtual display drivers are complex system components. Always backup your system before installation and have recovery options ready.

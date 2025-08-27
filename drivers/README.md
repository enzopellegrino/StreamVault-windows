# Driver Integration for Virtual Desktop Support

## Overview
StreamVault includes integrated support for virtual desktop creation using display drivers.

## Supported Drivers

### IDD Sample Driver
- **Purpose**: Creates virtual displays that appear as real monitors to Windows
- **Installation**: Automatic via StreamVault application
- **Requirements**: Administrator privileges
- **Compatibility**: Windows 10/11 x64

### Spacedesk Driver (Future Support)
- **Purpose**: Network-based virtual display solution
- **Status**: Planned for future implementation
- **Benefits**: Remote display capabilities

## Driver Files

### IddSampleDriver.inf
Windows driver information file that defines:
- Device identification
- Service installation
- Registry entries
- File copying instructions

### IddSampleDriver.dll (Required)
The actual driver binary file. This needs to be:
1. Downloaded from Microsoft IDD Sample Driver project
2. Compiled for your system architecture
3. Placed in the `drivers` folder

## Installation Process

1. **Automatic Detection**: StreamVault checks for driver availability
2. **Administrator Prompt**: Requests elevation for driver installation
3. **Driver Installation**: Uses Windows PnPUtil to install the driver
4. **Device Creation**: Creates virtual display devices
5. **Monitor Registration**: Registers new displays with Windows

## Security Notes

- Driver installation requires administrator privileges
- All drivers are verified before installation
- StreamVault only installs signed or test-signed drivers
- Installation can be reverted through Device Manager

## Troubleshooting

### Common Issues
1. **Access Denied**: Run StreamVault as Administrator
2. **Driver Not Found**: Ensure IddSampleDriver.dll is in drivers folder
3. **Installation Failed**: Check Windows Event Log for details
4. **No Virtual Displays**: Restart the application or system

### Verification Steps
1. Open Device Manager
2. Expand "Display adapters"
3. Look for "IDD Sample Driver" entries
4. Check Display Settings for new monitors

## Integration with StreamVault

The virtual desktop system integrates seamlessly with:
- **Monitor Selection**: Virtual displays appear in monitor lists
- **Screen Capture**: Full capture support for virtual displays
- **SRT Streaming**: Stream virtual desktop content
- **Multi-Stream**: Combine real and virtual displays

## Developer Notes

For developers working with this system:
- Driver service is implemented in `VirtualDesktopDriverService.cs`
- UI management in `VirtualDesktopManagerDialog.cs`
- Virtual desktop info model in `VirtualDesktopInfo.cs`
- Windows API integration for driver management

## Legal Notes

- IDD Sample Driver is provided by Microsoft under MIT license
- Spacedesk drivers are proprietary to datronicsoft
- StreamVault driver integration is open source
- Users are responsible for driver licensing compliance

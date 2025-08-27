# 🖥️ StreamVault v1.3.0 - Professional Screen Capture & SRT Streaming

[![Version](https://img.shields.io/badge/version-1.3.0-blue.svg)](https://github.com/enzopellegrino/StreamVault-windows/releases)
[![Platform](https://img.shields.io/badge/platform-Windows%2010%2F11-lightgrey.svg)]()
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)]()

## 🆕 Novità v1.3.0 - Virtual Desktop Support

**Creazione di Desktop Virtuali**: StreamVault ora include supporto completo per la creazione di desktop virtuali utilizzando driver integrati:

### 🖥️ Virtual Desktop Features
- **Driver IddSampleDriver integrato** per la creazione di display virtuali
- **Gestione automatica dei driver** con installazione guidata
- **Desktop virtuali personalizzabili** (risoluzione, frequenza di aggiornamento, colore)
- **Interfaccia di gestione** con preview e controllo completo
- **Integrazione seamless** con il sistema di streaming esistente

### 🔧 Gestione Driver
- **Verifica automatica** dei privilegi amministratore
- **Installazione driver** tramite Windows PnPUtil
- **Supporto per IddSampleDriver** (incluso nel pacchetto)
- **Supporto futuro** per Spacedesk e altri driver
- **Rimozione sicura** e gestione degli errori

---

## 🆕 Changelog v1.3.0

### ✨ Nuove Funzionalità
- **Virtual Desktop Support**: Integrazione completa driver IddSampleDriver
- **Driver Manager**: Interfaccia per gestione driver virtual display
- **Desktop Creator**: Creazione desktop virtuali personalizzati
- **Enhanced Security**: Gestione privilegi amministratore migliorata

### 🔧 Miglioramenti
- **Driver Integration**: Supporto automatico per driver virtual display
- **UI Enhancement**: Nuove interfacce per gestione desktop virtuali
- **Error Handling**: Gestione errori migliorata per driver operations
- **Documentation**: Documentazione estesa per virtual desktop features

### 🐛 Bug Fix
- **Driver Conflicts**: Risolti conflitti tra classi VirtualDesktopInfo
- **Installation Issues**: Migliorata gestione installazione driver
- **UI Responsiveness**: Migliorata reattività interfaccia durante operazioni driver

---

## 🔮 Roadmap Future

### 🚀 Prossime Versioni
- **v1.4.0**: Supporto Spacedesk driver integrato
- **v1.5.0**: Audio capture e streaming
- **v1.6.0**: Remote control e API REST
- **v2.0.0**: Cloud streaming e gestione remota

### 💡 Funzionalità in Valutazione
- **NDI Support**: Integrazione NewTek NDI protocol
- **RTMP Streaming**: Supporto diretto per RTMP (YouTube, Twitch)
- **AI Enhancement**: Upscaling e miglioramento qualità AI-powered
- **Mobile App**: Controllo remoto tramite app mobile

---

## Features

- **Multi-Monitor Support**: Detect and select from available monitors/displays
- **Virtual Monitor Creation**: Create virtual displays for streaming purposes
- **Configurable Settings**: Adjust FPS (1-60), bitrate (100-50000 kbps), and SRT URL
- **Real-time Streaming**: Low-latency screen capture and streaming
- **User-friendly Interface**: Simple WinForms GUI with intuitive controls
- **Logging**: Comprehensive logging for troubleshooting and monitoring
- **Error Handling**: Robust error handling with user-friendly messages

## Prerequisites

- **Windows 10/11** (x64)
- **.NET 8.0 Runtime** or later
- **FFmpeg** installed and accessible via PATH
- **Virtual Display Driver** (optional, for virtual monitor creation)

### Installing FFmpeg

1. Download FFmpeg from [https://ffmpeg.org/download.html](https://ffmpeg.org/download.html)
2. Extract the archive to a folder (e.g., `C:\ffmpeg`)
3. Add the FFmpeg `bin` folder to your system PATH environment variable
4. Verify installation by opening Command Prompt and running: `ffmpeg -version`

### Installing Virtual Display Drivers (Optional)

For virtual monitor creation, you can install one of these drivers:

1. **IddSampleDriver** (Recommended):
   - Download from [Microsoft Windows Driver Samples](https://github.com/microsoft/Windows-driver-samples)
   - Follow the installation instructions in the repository
   - Allows creation of virtual displays programmatically

2. **Spacedesk Driver**:
   - Download from [spacedesk.net](https://www.spacedesk.net)
   - Install the driver component
   - Provides virtual display functionality

3. **Virtual Display Manager**:
   - Various third-party solutions available
   - Check compatibility with your Windows version

## Building the Application

### Prerequisites for Building

- **Visual Studio 2022** or later with .NET desktop development workload
- **.NET 8.0 SDK** or later
- **NSIS** (for EXE installer creation)

### 🚀 Automated Build + EXE Installer (Recommended)
```bash
./build_and_package.sh
```
This command:
- ✅ Cleans obsolete files
- ✅ Compiles the project for Windows
- ✅ Creates professional EXE installer
- ✅ Output: `StreamVault-Latest/StreamVault_Setup_v1.0.0.exe` (ready for distribution)

### 📦 From VS Code
- **Cmd+Shift+P** → "Tasks: Run Task" → **"build-installer"**

### Manual Build Steps

1. Clone or download the source code
2. Open Command Prompt in the project directory
3. Run the following commands:

```bash
# Restore dependencies
dotnet restore

# Build the application
dotnet build --configuration Release

# Publish as self-contained executable (optional)
dotnet publish --configuration Release --self-contained true --runtime win-x64
```

### Running from Source

```bash
dotnet run
```

## Installation

### Using the Installer (Recommended)

1. Download the latest `StreamVault_Setup_v1.0.0.exe` from releases
2. Run the installer as Administrator
3. Follow the installation wizard
4. The application will be installed to `Program Files\StreamVault`

### Manual Installation

1. Download the latest release archive
2. Extract to your desired location
3. Ensure FFmpeg is installed and in PATH
4. Run `StreamVault.exe`

## Usage

### Basic Streaming

1. **Launch StreamVault**
2. **Select Monitor**: Choose from available displays or create a virtual one
3. **Configure Settings**:
   - **SRT URL**: Enter destination (e.g., `srt://192.168.1.100:9999`)
   - **FPS**: Set frame rate (1-60)
   - **Bitrate**: Set quality (100-50000 kbps)
4. **Start Streaming**: Click "Start Stream" button
5. **Monitor Status**: Watch the status indicators for stream health

### Virtual Monitor Management

1. **Create Virtual Monitor**: Click "Create Virtual Monitor"
2. **Configure Resolution**: Set desired width and height
3. **Manage Virtual Displays**: Use "Virtual Monitor List" to view/remove
4. **Chrome Integration**: Automatically opens Chrome on virtual displays

### Multi-Monitor Streaming

1. **Open Multi-Stream**: Click "Multi Monitor Stream"
2. **Configure Each Stream**: Set individual SRT URLs and settings
3. **Start All Streams**: Begin simultaneous streaming from multiple monitors
4. **Monitor Performance**: Track all streams in unified interface

## Configuration

### Settings File

StreamVault automatically saves configuration to:
```
%APPDATA%\StreamVault\config.json
```

### Supported Settings

- **Monitor Selection**: Last used monitor/display
- **Stream Settings**: FPS, bitrate, SRT URL
- **Window Position**: Application window placement
- **Virtual Monitor**: Preferred resolutions and settings

## Troubleshooting

### Common Issues

1. **FFmpeg not found**:
   - Ensure FFmpeg is installed and in system PATH
   - Try running `ffmpeg -version` in Command Prompt
   - Use the FFmpeg auto-download feature in the application

2. **Streaming fails to start**:
   - Verify the SRT URL is correct and accessible
   - Check firewall settings
   - Review logs in `%APPDATA%\StreamVault` for detailed error information

3. **"No monitors detected"**:
   - Check Windows display settings
   - Try clicking "Refresh" button in the Monitor Selection section

4. **Virtual monitor creation fails**:
   - Ensure virtual display driver is installed (IddSampleDriver recommended)
   - Check Windows Device Manager for virtual display devices
   - Run application as Administrator if required
   - Review logs for detailed error information

5. **Poor streaming quality**:
   - Increase bitrate for better quality (higher bandwidth required)
   - Reduce FPS for unstable connections
   - Check network connectivity to destination

6. **Application crashes**:
   - Check logs in `%APPDATA%\StreamVault\logs`
   - Ensure .NET 8.0 runtime is installed
   - Try running as Administrator

### Debug Tools

StreamVault includes built-in debugging features:

1. **FFmpeg Debug**: Test FFmpeg installation and capture capabilities
2. **SRT Stream Test**: Verify SRT connectivity and streaming
3. **Device Enumeration**: List available capture devices
4. **Real-time Output**: View FFmpeg output in real-time

### Logging

Comprehensive logging is available:
- **Application Logs**: `%APPDATA%\StreamVault\logs`
- **FFmpeg Output**: Visible via Debug tools
- **Error Details**: Detailed error information for troubleshooting

## Architecture

### Components

- **MainForm**: Primary application interface
- **MultiStreamForm**: Multi-monitor streaming management
- **ScreenCaptureService**: Handles screen capture operations
- **StreamingService**: Manages FFmpeg streaming processes
- **VirtualDisplayService**: Virtual monitor creation and management
- **ChromeManagementService**: Chrome browser automation
- **ConfigurationService**: Settings persistence and management

### Technologies

- **.NET 8.0**: Core framework
- **Windows Forms**: User interface
- **FFmpeg**: Video processing and streaming
- **SRT Protocol**: Reliable streaming transport
- **Windows API**: System integration for displays and processes

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- **FFmpeg**: For powerful video processing capabilities
- **SRT Alliance**: For the SRT protocol specification
- **Microsoft**: For .NET framework and Windows development tools

## Support

For issues, questions, or contributions:
- Open an issue on GitHub
- Check the troubleshooting section
- Review logs for detailed error information

## Features

- **Multi-Monitor Support**: Detect and select from available monitors/displays
- **Virtual Monitor Creation**: Create virtual displays for streaming purposes
- **Configurable Settings**: Adjust FPS (1-60), bitrate (100-50000 kbps), and SRT URL
- **Real-time Streaming**: Low-latency screen capture and streaming
- **User-friendly Interface**: Simple WinForms GUI with intuitive controls
- **Logging**: Comprehensive logging for troubleshooting and monitoring
- **Error Handling**: Robust error handling with user-friendly messages

## Prerequisites

- **Windows 10/11** (x64)
- **.NET 8.0 Runtime** or later
- **FFmpeg** installed and accessible via PATH
- **Virtual Display Driver** (optional, for virtual monitor creation)

### Installing FFmpeg

1. Download FFmpeg from [https://ffmpeg.org/download.html](https://ffmpeg.org/download.html)
2. Extract the archive to a folder (e.g., `C:\ffmpeg`)
3. Add the FFmpeg `bin` folder to your system PATH environment variable
4. Verify installation by opening Command Prompt and running: `ffmpeg -version`

### Installing Virtual Display Drivers (Optional)

For virtual monitor creation, you can install one of these drivers:

1. **IddSampleDriver** (Recommended):
   - Download from [Microsoft Windows Driver Samples](https://github.com/microsoft/Windows-driver-samples)
   - Follow the installation instructions in the repository
   - Allows creation of virtual displays programmatically

2. **Spacedesk Driver**:
   - Download from [spacedesk.net](https://www.spacedesk.net)
   - Install the driver component
   - Provides virtual display functionality

3. **Virtual Display Manager**:
   - Various third-party solutions available
   - Check compatibility with your Windows version

## Building the Application

### Prerequisites for Building

- **Visual Studio 2022** or later with .NET desktop development workload
- **.NET 8.0 SDK** or later
- **NSIS** (for EXE installer creation)

### 🚀 Build Automatico + Installer EXE (Raccomandato)
```bash
./build_and_package.sh
```
Questo comando:
- ✅ Pulisce file obsoleti
- ✅ Compila il progetto per Windows
- ✅ Crea installer EXE professionale
- ✅ Output: `StreamVault-Latest/StreamVault_Setup_v1.0.0.exe` (pronto per distribuzione)

### 📦 Da VS Code
- **Cmd+Shift+P** → "Tasks: Run Task" → **"build-installer"**

### Build Steps (Manuale)

1. Clone or download the source code
2. Open Command Prompt in the project directory
3. Run the following commands:

```bash
# Restore dependencies
dotnet restore

# Build the application
dotnet build --configuration Release

# Publish as self-contained executable (optional)
dotnet publish --configuration Release --self-contained true --runtime win-x64
```

### Running from Source

```bash
dotnet run
```

## Using the Application

1. **Launch StreamVault**: Run the executable or use `dotnet run`
2. **Select Monitor**: Choose which monitor/display to capture from the dropdown
3. **Virtual Monitors** (Optional):
   - Click "Create Virtual" to add a virtual monitor
   - Configure resolution and settings
   - Remove virtual monitors when no longer needed
4. **Configure Settings**:
   - **SRT URL**: Enter your SRT destination (e.g., `srt://192.168.1.100:9999`)
   - **FPS**: Set frames per second (1-60)
   - **Bitrate**: Set video bitrate in kbps (100-50000)
5. **Start Streaming**: Click "Start Streaming" to begin capture and streaming
6. **Monitor Status**: Watch the status indicator for streaming state
7. **Stop Streaming**: Click "Stop Streaming" to end the session

### SRT URL Examples

- **Local streaming**: `srt://127.0.0.1:9999`
- **Remote streaming**: `srt://192.168.1.100:9999`
- **With parameters**: `srt://example.com:9999?mode=caller&latency=120`

## Troubleshooting

### Common Issues

1. **"FFmpeg not found" error**:
   - Ensure FFmpeg is installed and in your system PATH
   - Try running `ffmpeg -version` in Command Prompt

2. **"No monitors detected"**:
   - Check Windows display settings
   - Try clicking "Refresh" button in the Monitor Selection section

3. **Streaming fails to start**:
   - Verify the SRT URL is correct and accessible
   - Check firewall settings
   - Review logs in `%APPDATA%\StreamVault\` for detailed error information

4. **Poor streaming quality**:
   - Adjust bitrate settings (higher = better quality, more bandwidth)
   - Lower FPS for better stability
   - Ensure sufficient network bandwidth

### Log Files

Application logs are stored in:
```
%APPDATA%\StreamVault\StreamVault_YYYYMMDD.log
```

These logs contain detailed information about application events, errors, and FFmpeg output.

## FFmpeg Command Reference

The application generates FFmpeg commands similar to:

```bash
ffmpeg -f gdigrab -framerate 30 -offset_x 0 -offset_y 0 -video_size 1920x1080 -i desktop -c:v libx264 -preset ultrafast -tune zerolatency -pix_fmt yuv420p -b:v 2000k -bufsize 4000k -maxrate 2000k -g 30 -keyint_min 30 -sc_threshold 0 -fflags +genpts -f mpegts "srt://127.0.0.1:9999?pkt_size=1316&mode=caller"
```

## System Requirements

- **OS**: Windows 10 (1903) or Windows 11
- **CPU**: Intel i5 or AMD Ryzen 5 (minimum), i7/Ryzen 7 recommended for higher bitrates
- **RAM**: 4GB minimum, 8GB recommended
- **Storage**: 100MB for application, additional space for logs
- **Network**: Sufficient bandwidth for chosen bitrate (e.g., 2Mbps for 2000kbps stream)

## Performance Tips

- **Lower FPS** (15-24) for better stability on slower systems
- **Reduce bitrate** if experiencing network issues
- **Close unnecessary applications** to free up system resources
- **Use wired network connection** for best streaming stability

## License

This project is provided as-is for educational and personal use.

## Support

For issues, questions, or feature requests, check the application logs first, then review the troubleshooting section above.

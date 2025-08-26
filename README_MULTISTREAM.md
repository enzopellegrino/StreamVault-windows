# 🎥 StreamVault - Multi-Monitor Screen Capture & SRT Streaming

**StreamVault** is now an advanced Windows application for **simultaneous multi-monitor capture** and **SRT streaming** with automatic Chrome management.

## 🚀 Key Features

### ✅ Single Monitor Streaming (Classic Mode)
- **Screen capture** of a single monitor
- **SRT streaming** with integrated FFmpeg
- **Simulated virtual monitors**
- **Manual configuration** of FPS, bitrate and SRT URL

### 🆕 Multi-Monitor Streaming (NEW!)
- **🎯 Simultaneous streaming** of ALL monitors
- **🌐 Automatic Chrome** on every monitor
- **📡 Automatic SRT URLs** (incremental port)
- **⚡ Centralized management** of all streams
- **📊 Real-time monitoring** of every stream

## 📋 Multi-Stream Architecture

### How It Works
1. **Monitor Detection**: The app automatically detects all connected monitors
2. **Chrome Launch**: Launches Chrome in fullscreen on every monitor
3. **Automatic URLs**: Generates incremental SRT URLs (9999, 10000, 10001...)
4. **Parallel Streams**: Starts separate FFmpeg for every monitor
5. **Centralized Control**: Manages everything from a single interface

### Example Setup
```
Monitor 1: 1920x1080 → Chrome → FFmpeg → srt://127.0.0.1:9999
Monitor 2: 1920x1080 → Chrome → FFmpeg → srt://127.0.0.1:10000  
Monitor 3: 1440x900  → Chrome → FFmpeg → srt://127.0.0.1:10001
```

## 🔧 Installation and Requirements

### Prerequisites
- **Windows 10/11** (x64)
- **Google Chrome** installed
- **FFmpeg** installed ([Download](https://ffmpeg.org/download.html))

### Quick Install FFmpeg
```cmd
# Chocolatey
choco install ffmpeg

# Scoop  
scoop install ffmpeg

# Manual
# Download from https://ffmpeg.org
# Extract to C:\ffmpeg\
# Add C:\ffmpeg\bin to PATH
```

### Execution
1. Download `StreamVault.exe` from `publish-multistream/`
2. Ensure Chrome and FFmpeg are installed
3. Run `StreamVault.exe`

## 🎮 Usage

### Single Monitor Mode
1. **Open StreamVault** → classic interface
2. **Select monitor** from dropdown
3. **Configure SRT URL** (e.g. `srt://127.0.0.1:9999`)
4. **Set FPS/Bitrate** 
5. **Start Streaming** → FFmpeg captures the selected monitor

### 🆕 Multi-Monitor Mode
1. **Open StreamVault** → click **"🎥 Multi-Monitor Streaming"**
2. **Automatic configuration**:
   - Base Host: `127.0.0.1`
   - Base Port: `9999` (incremental for each monitor)
   - Chrome URL: `https://www.google.com`
   - Auto-start Chrome: ✅ enabled
3. **Verify setup** in Stream Sessions table
4. **"Start All Streams"** → 
   - Chrome starts on all monitors
   - FFmpeg captures each monitor separately
   - Parallel SRT streams active

### Advanced Controls
- **"Refresh Monitors"**: Reload monitor list
- **"Generate SRT URLs"**: Regenerate URLs with new settings
- **"Test Chrome"**: Verify Chrome installation
- **Stream Sessions Table**: Real-time monitoring of each stream

## 🛠️ Advanced Configuration

### Custom SRT URLs
```
Base Host: 192.168.1.100
Base Port: 8000
→ 
Monitor 1: srt://192.168.1.100:8000
Monitor 2: srt://192.168.1.100:8001
Monitor 3: srt://192.168.1.100:8002
```

### FFmpeg Parameters Per Monitor
Each stream uses optimized configuration:
```bash
-f gdigrab -framerate 30 -i "\\.\DISPLAY1" 
-c:v libx264 -preset ultrafast -tune zerolatency 
-b:v 2000k -maxrate 4000k -bufsize 8000k 
-pix_fmt yuv420p -f mpegts "srt://destination"
```

### Chrome Positioning
- **Automatic fullscreen** on target monitor
- **Precise positioning** with Windows API
- **Customizable URL** for each instance
- **Process management** for automatic cleanup

## 📊 Monitoring and Status

### Multi-Stream Interface
- **General status**: Ready/Streaming/Error
- **Detailed table**: Monitor, SRT URL, FPS, Bitrate, Status per stream
- **Real-time counters**: Active streams, Chrome active
- **Integrated log**: Everything tracked in logging service

### Status Indicators
- 🟢 **Green**: Stream active and working
- 🟡 **Yellow**: Stream starting
- 🔴 **Red**: Stream error
- ⚫ **Black**: Stream stopped

## 🐛 Troubleshooting

### Chrome won't start
```
Error: Chrome executable not found
```
**Solutions**:
1. Install Google Chrome
2. Use "Test Chrome" to verify
3. Chrome must be in standard paths

### FFmpeg errors
```
Error: FFmpeg executable not found  
```
**Solutions**:
1. Install FFmpeg and add to PATH
2. Or copy `ffmpeg.exe` to app directory
3. Test with `ffmpeg -version` in cmd

### Stream won't start
**Checks**:
1. **SRT URLs** correct and ports free
2. **Firewall** allows SRT traffic
3. **Monitors** active and detected
4. **Chrome** can access URLs

### Performance Issues
**Optimizations**:
1. **Reduce FPS**: 30 → 15 for all streams
2. **Reduce Bitrate**: 2000 → 1000 kbps
3. **Close apps** not needed during streaming
4. **Hardware**: Multi-core CPU recommended for multi-stream

## 📁 Project Structure

### New Components
```
StreamVault/
├── Forms/
│   ├── MainForm.cs              # Classic interface + launcher
│   └── MultiStreamForm.cs       # 🆕 Multi-monitor interface
├── Services/
│   ├── FFmpegService.cs         # FFmpeg integration
│   ├── MultiStreamService.cs    # 🆕 Multiple stream management
│   └── ChromeManagementService.cs # 🆕 Multi-monitor Chrome management
├── Models/
│   ├── MultiStreamConfig.cs     # 🆕 Multi-stream configuration
│   └── StreamSession.cs         # 🆕 Individual streaming session
└── StreamVault.exe              # Final executable
```

### Service Architecture
- **MultiStreamService**: Coordinates multiple streams
- **ChromeManagementService**: Chrome management for monitors
- **FFmpegService**: FFmpeg process for streams
- **VirtualDisplayService**: Simulated virtual monitors

## 🔄 Typical Use Cases

### Multi-Cam Streaming Setup
```
Studio setup with 3 monitors:
- Monitor 1: Director/control  → Main stream
- Monitor 2: Presentation     → Secondary stream  
- Monitor 3: Chat/comments    → Tertiary stream

→ 3 parallel SRT streams for video mixer
```

### Digital Signage
```
Display network:
- Each monitor shows different web content
- Centralized streaming to server
- Remote monitoring of all displays
```

### Event Broadcasting
```
Conference setup:
- Speaker monitor → Main stream
- Slides monitor → Slides stream
- Audience monitor → Environment stream

→ Mixer receives 3 separate streams
```

## 🎯 Future Roadmap

### v2.0 Planned Features
- [ ] **Audio Capture** integrated for each stream
- [ ] **Local recording** in addition to streaming
- [ ] **Saveable stream presets**
- [ ] **Web dashboard** for remote control
- [ ] **Automatic load balancing**
- [ ] **Real virtual monitors** with IDD driver

### v2.1 Advanced Features  
- [ ] **Built-in stream mixing**
- [ ] **Automatic bandwidth adaptation**
- [ ] **Stream failover** and redundancy
- [ ] **Cloud integration** for storage
- [ ] **Mobile app** for remote control

---

## 🎬 Demo Quick Start

1. **Install**: Chrome + FFmpeg
2. **Run**: `StreamVault.exe`
3. **Click**: "🎥 Multi-Monitor Streaming"
4. **Verify**: Monitors detected in table
5. **Start**: "Start All Streams"
6. **Result**: Chrome on all monitors + active SRT streams!

**StreamVault v2.0** - *Multi-Monitor Streaming made simple* 🎥✨🚀

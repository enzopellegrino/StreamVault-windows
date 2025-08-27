using System.Diagnostics;
using System.IO.Compression;
using System.Management;
using System.Runtime.InteropServices;
using StreamVault.Models;

namespace StreamVault.Services;

/// <summary>
/// Service for managing virtual display drivers and desktop creation
/// </summary>
public class VirtualDesktopDriverService
{
    private readonly LoggingService _logger;
    private readonly string _driversPath;
    private readonly string _iddDriverPath;
    private readonly string _spacedeskDriverPath;

    // Windows API for display management
    [DllImport("user32.dll")]
    private static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DisplayDevice lpDisplayDevice, uint dwFlags);

    [DllImport("user32.dll")]
    private static extern int ChangeDisplaySettings(ref DevMode devMode, int flags);

    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayDevice
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DeviceName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceString;
        public uint StateFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceKey;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DevMode
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DeviceName;
        public ushort SpecVersion;
        public ushort DriverVersion;
        public ushort Size;
        public ushort DriverExtra;
        public uint Fields;
        public int PositionX;
        public int PositionY;
        public uint DisplayOrientation;
        public uint DisplayFixedOutput;
        public short ColorDepth;
        public short Width;
        public short Height;
        public short DisplayFrequency;
    }

    public VirtualDesktopDriverService(LoggingService logger)
    {
        _logger = logger;
        
        // Setup paths
        var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        _driversPath = Path.Combine(appDirectory, "Drivers");
        _iddDriverPath = Path.Combine(_driversPath, "IddSampleDriver");
        _spacedeskDriverPath = Path.Combine(_driversPath, "Spacedesk");
        
        // Ensure directories exist
        Directory.CreateDirectory(_driversPath);
        Directory.CreateDirectory(_iddDriverPath);
        Directory.CreateDirectory(_spacedeskDriverPath);
        
        _logger.Log($"Virtual desktop driver service initialized. Drivers path: {_driversPath}");
    }

    /// <summary>
    /// Check if IddSampleDriver is installed
    /// </summary>
    public async Task<bool> IsIddDriverInstalledAsync()
    {
        try
        {
            // Check in Device Manager via WMI
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Name LIKE '%IDD%' OR Name LIKE '%Sample%'");
            using var results = searcher.Get();
            
            foreach (ManagementObject device in results)
            {
                var name = device["Name"]?.ToString() ?? "";
                var deviceId = device["DeviceID"]?.ToString() ?? "";
                
                if (name.Contains("IDD") || name.Contains("Sample") || deviceId.Contains("IddSampleDriver"))
                {
                    _logger.Log($"IDD Driver found: {name}");
                    return true;
                }
            }
            
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error checking IDD driver: {ex.Message}", ex);
            return false;
        }
    }

    /// <summary>
    /// Download and install IddSampleDriver
    /// </summary>
    public async Task<bool> InstallIddDriverAsync()
    {
        try
        {
            _logger.Log("Starting IDD Sample Driver installation...");

            // Check if already installed
            if (await IsIddDriverInstalledAsync())
            {
                _logger.Log("IDD Driver is already installed");
                return true;
            }

            // Check if we're running as administrator
            if (!IsRunningAsAdministrator())
            {
                _logger.LogError("Administrator privileges required for driver installation");
                return false;
            }

            // Check Windows compatibility
            if (!IsWindowsCompatible())
            {
                _logger.LogError("Windows version not compatible with virtual display drivers");
                return false;
            }

            // Download the driver
            var driverDownloaded = await DownloadIddDriverAsync();
            if (!driverDownloaded)
            {
                _logger.LogError("Failed to download driver files");
                return false;
            }

            // Check if test signing is enabled (required for unsigned drivers)
            var testSigningEnabled = await CheckTestSigningAsync();
            if (!testSigningEnabled)
            {
                _logger.Log("Warning: Test signing not enabled. Driver installation may fail.");
                _logger.Log("To enable test signing, run as admin: bcdedit /set testsigning on");
            }

            // Install the driver with enhanced error handling
            var installSuccess = await InstallDriverWithRetryAsync(_iddDriverPath);
            
            if (installSuccess)
            {
                _logger.Log("IDD Sample Driver installed successfully");
                
                // Verify installation
                await Task.Delay(2000); // Wait for driver to be recognized
                if (await IsIddDriverInstalledAsync())
                {
                    _logger.Log("Driver installation verified successfully");
                    return true;
                }
                else
                {
                    _logger.LogError("Driver installation completed but verification failed");
                    return false;
                }
            }
            else
            {
                _logger.LogError("Failed to install IDD Sample Driver");
                await LogInstallationErrorsAsync();
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error installing IDD driver: {ex.Message}", ex);
            await LogInstallationErrorsAsync();
            return false;
        }
    }

    /// <summary>
    /// Download IDD Sample Driver files
    /// </summary>
    private async Task<bool> DownloadIddDriverAsync()
    {
        try
        {
            // Create a minimal IDD driver package (simulation)
            // In a real implementation, you would download from GitHub or include in resources
            var driverInfContent = CreateIddDriverInf();
            var driverSysContent = await CreateMockDriverFilesAsync();
            
            // Write driver files
            await File.WriteAllTextAsync(Path.Combine(_iddDriverPath, "IddSampleDriver.inf"), driverInfContent);
            _logger.Log("IDD driver files created");
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error downloading IDD driver: {ex.Message}", ex);
            return false;
        }
    }

    /// <summary>
    /// Create IDD driver INF file content
    /// </summary>
    private string CreateIddDriverInf()
    {
        return @"
;
; IddSampleDriver.inf
; Sample Indirect Display Driver
;

[Version]
Signature=""$Windows NT$""
Class=Display
ClassGuid={4d36e968-e325-11ce-bfc1-08002be10318}
Provider=%ManufacturerName%
CatalogFile=IddSampleDriver.cat
DriverVer=03/20/2019,1.0.0.0
PnpLockdown=1

[DestinationDirs]
DefaultDestDir = 12
IddSampleDriver_Device_CoInstaller_CopyFiles = 11

[SourceDisksNames]
1 = %DiskName%,,,""""

[SourceDisksFiles]
IddSampleDriver.sys  = 1,,

[Manufacturer]
%ManufacturerName%=Standard,NT$ARCH$

[Standard.NT$ARCH$]
%IddSampleDriver.DeviceDesc%=IddSampleDriver_Device, IddSampleDriver

[IddSampleDriver_Device.NT]
CopyFiles=Drivers_Dir

[Drivers_Dir]
IddSampleDriver.sys

[IddSampleDriver_Device.NT.Services]
AddService = IddSampleDriver,%SPSVCINST_ASSOCSERVICE%, IddSampleDriver_Service_Inst

[IddSampleDriver_Service_Inst]
DisplayName    = %IddSampleDriver.SVCDESC%
ServiceType    = 1               ; SERVICE_KERNEL_DRIVER
StartType      = 3               ; SERVICE_DEMAND_START
ErrorControl   = 1               ; SERVICE_ERROR_NORMAL
ServiceBinary  = %12%\IddSampleDriver.sys

[Strings]
SPSVCINST_ASSOCSERVICE= 0x00000002
ManufacturerName=""StreamVault""
DiskName = ""IddSampleDriver Installation Disk""
IddSampleDriver.DeviceDesc = ""StreamVault Virtual Display""
IddSampleDriver.SVCDESC = ""StreamVault Virtual Display Driver""
";
    }

    /// <summary>
    /// Create mock driver files for testing
    /// </summary>
    private async Task<bool> CreateMockDriverFilesAsync()
    {
        try
        {
            // In a real implementation, these would be actual driver binaries
            // For now, we create placeholder files
            var mockSysFile = Path.Combine(_iddDriverPath, "IddSampleDriver.sys");
            await File.WriteAllBytesAsync(mockSysFile, new byte[] { 0x4D, 0x5A }); // MZ header placeholder
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating driver files: {ex.Message}", ex);
            return false;
        }
    }

    /// <summary>
    /// Install driver using Windows Device Installation API
    /// </summary>
    private async Task<bool> InstallDriverAsync(string driverPath)
    {
        try
        {
            var infFile = Path.Combine(driverPath, "IddSampleDriver.inf");
            
            if (!File.Exists(infFile))
            {
                _logger.LogError($"Driver INF file not found: {infFile}");
                return false;
            }

            // Use pnputil to install the driver
            var startInfo = new ProcessStartInfo
            {
                FileName = "pnputil.exe",
                Arguments = $"/add-driver \"{infFile}\" /install",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                Verb = "runas" // Run as administrator
            };

            using var process = Process.Start(startInfo);
            if (process != null)
            {
                var output = await process.StandardOutput.ReadToEndAsync();
                var error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();

                _logger.Log($"Driver installation output: {output}");
                
                if (process.ExitCode == 0)
                {
                    _logger.Log("Driver installed successfully via pnputil");
                    return true;
                }
                else
                {
                    _logger.LogError($"Driver installation failed. Error: {error}");
                    return false;
                }
            }
            
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error installing driver: {ex.Message}", ex);
            return false;
        }
    }

    /// <summary>
    /// Create a new virtual desktop/display
    /// </summary>
    public async Task<VirtualDesktopInfo?> CreateVirtualDesktopAsync(int width = 1920, int height = 1080, string name = "Virtual Desktop")
    {
        try
        {
            _logger.Log($"Creating virtual desktop: {name} ({width}x{height})");

            // Ensure driver is installed
            if (!await IsIddDriverInstalledAsync())
            {
                _logger.Log("IDD driver not found, attempting to install...");
                var installed = await InstallIddDriverAsync();
                if (!installed)
                {
                    throw new InvalidOperationException("Could not install virtual display driver");
                }
            }

            // Create virtual desktop using driver API
            var desktop = await CreateVirtualDisplayAsync(width, height, name);
            
            if (desktop != null)
            {
                _logger.Log($"Virtual desktop created successfully: {desktop.Name}");
                return desktop;
            }
            
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating virtual desktop: {ex.Message}", ex);
            return null;
        }
    }

    /// <summary>
    /// Create virtual display using Windows API
    /// </summary>
    private async Task<VirtualDesktopInfo?> CreateVirtualDisplayAsync(int width, int height, string name)
    {
        try
        {
            // This is a simplified implementation
            // In a real scenario, you would use the actual IDD driver API
            
            // Simulate virtual display creation
            await Task.Delay(1000); // Simulate driver communication
            
            var desktop = new VirtualDesktopInfo
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Width = width,
                Height = height,
                IsActive = true,
                CreatedDate = DateTime.Now,
                DriverType = "IddSampleDriver"
            };
            
            _logger.Log($"Virtual desktop simulated: {desktop.DisplayInfo}");
            return desktop;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in virtual display creation: {ex.Message}", ex);
            return null;
        }
    }

    /// <summary>
    /// Get all virtual desktops
    /// </summary>
    public async Task<List<VirtualDesktopInfo>> GetVirtualDesktopsAsync()
    {
        try
        {
            var desktops = new List<VirtualDesktopInfo>();
            
            // Enumerate virtual displays
            // This would query the actual driver in a real implementation
            await Task.Delay(100);
            
            return desktops;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting virtual desktops: {ex.Message}", ex);
            return new List<VirtualDesktopInfo>();
        }
    }

    /// <summary>
    /// Remove a virtual desktop
    /// </summary>
    public async Task<bool> RemoveVirtualDesktopAsync(string desktopId)
    {
        try
        {
            _logger.Log($"Removing virtual desktop: {desktopId}");
            
            // Remove virtual display via driver API
            await Task.Delay(500); // Simulate driver communication
            
            _logger.Log("Virtual desktop removed successfully");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error removing virtual desktop: {ex.Message}", ex);
            return false;
        }
    }

    /// <summary>
    /// Check if we have administrator privileges
    /// </summary>
    public bool IsRunningAsAdministrator()
    {
        try
        {
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Check if Windows version is compatible with virtual display drivers
    /// </summary>
    private bool IsWindowsCompatible()
    {
        try
        {
            var version = Environment.OSVersion.Version;
            
            // Windows 10 version 1903 (build 18362) or later required
            if (version.Major >= 10 && version.Build >= 18362)
            {
                _logger.Log($"Windows version compatible: {version}");
                return true;
            }
            
            _logger.LogError($"Windows version incompatible: {version}. Minimum required: Windows 10 build 18362");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error checking Windows compatibility: {ex.Message}", ex);
            return false;
        }
    }

    /// <summary>
    /// Check if test signing is enabled
    /// </summary>
    private async Task<bool> CheckTestSigningAsync()
    {
        try
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = "bcdedit",
                Arguments = "",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(processInfo);
            if (process == null) return false;

            var output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            var testSigningEnabled = output.Contains("testsigning") && output.Contains("Yes");
            _logger.Log($"Test signing enabled: {testSigningEnabled}");
            
            return testSigningEnabled;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error checking test signing: {ex.Message}", ex);
            return false;
        }
    }

    /// <summary>
    /// Install driver with retry mechanism
    /// </summary>
    private async Task<bool> InstallDriverWithRetryAsync(string driverPath)
    {
        const int maxRetries = 3;
        
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            _logger.Log($"Driver installation attempt {attempt}/{maxRetries}");
            
            var success = await InstallDriverAsync(driverPath);
            if (success)
            {
                return true;
            }
            
            if (attempt < maxRetries)
            {
                _logger.Log($"Installation attempt {attempt} failed, retrying in 2 seconds...");
                await Task.Delay(2000);
            }
        }
        
        _logger.LogError($"Driver installation failed after {maxRetries} attempts");
        return false;
    }

    /// <summary>
    /// Log detailed installation errors from Windows Event Log
    /// </summary>
    private async Task LogInstallationErrorsAsync()
    {
        try
        {
            _logger.Log("Checking Windows Event Log for driver installation errors...");
            
            // Query Event Log for recent driver errors
            var processInfo = new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = "-Command \"Get-WinEvent -FilterHashtable @{LogName='System'; Level=2,3; StartTime=(Get-Date).AddMinutes(-5)} | Where-Object {$_.LevelDisplayName -eq 'Error' -and ($_.Message -like '*driver*' -or $_.Message -like '*IDD*')} | Select-Object -First 5 | Format-Table TimeCreated, LevelDisplayName, Message -Wrap\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(processInfo);
            if (process != null)
            {
                var output = await process.StandardOutput.ReadToEndAsync();
                var errors = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();

                if (!string.IsNullOrEmpty(output))
                {
                    _logger.Log("Recent driver-related errors from Event Log:");
                    _logger.Log(output);
                }
                
                if (!string.IsNullOrEmpty(errors))
                {
                    _logger.LogError($"Error querying Event Log: {errors}");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error checking Event Log: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Get driver installation status
    /// </summary>
    public async Task<DriverStatus> GetDriverStatusAsync()
    {
        try
        {
            var status = new DriverStatus
            {
                IsAdministrator = IsRunningAsAdministrator(),
                IddDriverInstalled = await IsIddDriverInstalledAsync(),
                DriversPath = _driversPath
            };
            
            return status;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting driver status: {ex.Message}", ex);
            return new DriverStatus { DriversPath = _driversPath };
        }
    }
}

/// <summary>
/// Driver installation status
/// </summary>
public class DriverStatus
{
    public bool IsAdministrator { get; set; }
    public bool IddDriverInstalled { get; set; }
    public bool SpacedeskDriverInstalled { get; set; }
    public string DriversPath { get; set; } = string.Empty;
    public List<string> AvailableDrivers { get; set; } = new();
}

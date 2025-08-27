using System;

namespace StreamVault.Models;

/// <summary>
/// Rappresenta le informazioni di un desktop virtuale
/// </summary>
public class VirtualDesktopInfo
{
    /// <summary>
    /// ID univoco del desktop virtuale
    /// </summary>
    public string Id { get; set; } = "";

    /// <summary>
    /// Nome descrittivo del desktop virtuale
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Larghezza del desktop virtuale in pixel
    /// </summary>
    public int Width { get; set; } = 1920;

    /// <summary>
    /// Altezza del desktop virtuale in pixel
    /// </summary>
    public int Height { get; set; } = 1080;

    /// <summary>
    /// Frequenza di aggiornamento in Hz
    /// </summary>
    public int RefreshRate { get; set; } = 60;

    /// <summary>
    /// Profondità di colore in bit
    /// </summary>
    public int ColorDepth { get; set; } = 32;

    /// <summary>
    /// Indica se il desktop virtuale è attivo
    /// </summary>
    public bool IsActive { get; set; } = false;

    /// <summary>
    /// Indica se il desktop virtuale è connesso
    /// </summary>
    public bool IsConnected { get; set; } = false;

    /// <summary>
    /// Indice del display nel sistema
    /// </summary>
    public int DisplayIndex { get; set; } = -1;

    /// <summary>
    /// Nome del driver utilizzato
    /// </summary>
    public string DriverName { get; set; } = "IddSampleDriver";

    /// <summary>
    /// Tipo di driver utilizzato
    /// </summary>
    public string DriverType { get; set; } = "IddSampleDriver";

    /// <summary>
    /// Data di creazione del desktop virtuale
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// Data di creazione del desktop virtuale (alias per compatibilità)
    /// </summary>
    public DateTime CreatedDate 
    { 
        get => CreatedAt; 
        set => CreatedAt = value; 
    }

    /// <summary>
    /// Posizione X del desktop nel sistema multi-monitor
    /// </summary>
    public int PositionX { get; set; } = 0;

    /// <summary>
    /// Posizione Y del desktop nel sistema multi-monitor
    /// </summary>
    public int PositionY { get; set; } = 0;

    /// <summary>
    /// Indica se il desktop è il monitor primario
    /// </summary>
    public bool IsPrimary { get; set; } = false;

    /// <summary>
    /// ID del dispositivo hardware
    /// </summary>
    public string DeviceId { get; set; } = "";

    /// <summary>
    /// Nome del dispositivo
    /// </summary>
    public string DeviceName { get; set; } = "";

    /// <summary>
    /// Descrizione del dispositivo
    /// </summary>
    public string DeviceDescription { get; set; } = "";

    /// <summary>
    /// Indica se il desktop supporta l'accelerazione hardware
    /// </summary>
    public bool SupportsHardwareAcceleration { get; set; } = true;

    /// <summary>
    /// Modalità di orientamento del display
    /// </summary>
    public DisplayOrientation Orientation { get; set; } = DisplayOrientation.Landscape;

    /// <summary>
    /// Conversione in stringa per debug
    /// </summary>
    public override string ToString()
    {
        return $"{Name} ({Width}x{Height}@{RefreshRate}Hz) - {(IsActive ? "Attivo" : "Inattivo")}";
    }

    /// <summary>
    /// Informazioni di display formattate
    /// </summary>
    public string DisplayInfo => $"{Name} ({Width}x{Height})";

    /// <summary>
    /// Converte le informazioni del desktop virtuale in MonitorInfo
    /// </summary>
    public MonitorInfo ToMonitorInfo()
    {
        return new MonitorInfo
        {
            DeviceName = string.IsNullOrEmpty(DeviceId) ? Id : DeviceId,
            FriendlyName = Name,
            Width = Width,
            Height = Height,
            Left = PositionX,
            Top = PositionY,
            IsPrimary = IsPrimary
        };
    }

    /// <summary>
    /// Crea una copia delle informazioni del desktop virtuale
    /// </summary>
    public VirtualDesktopInfo Clone()
    {
        return new VirtualDesktopInfo
        {
            Id = Id,
            Name = Name,
            Width = Width,
            Height = Height,
            RefreshRate = RefreshRate,
            ColorDepth = ColorDepth,
            IsActive = IsActive,
            IsConnected = IsConnected,
            DisplayIndex = DisplayIndex,
            DriverName = DriverName,
            CreatedAt = CreatedAt,
            PositionX = PositionX,
            PositionY = PositionY,
            IsPrimary = IsPrimary,
            DeviceId = DeviceId,
            DeviceName = DeviceName,
            DeviceDescription = DeviceDescription,
            SupportsHardwareAcceleration = SupportsHardwareAcceleration,
            Orientation = Orientation
        };
    }

    /// <summary>
    /// Verifica se due desktop virtuali sono equivalenti
    /// </summary>
    public bool Equals(VirtualDesktopInfo? other)
    {
        if (other == null) return false;
        return Id == other.Id && 
               DeviceId == other.DeviceId;
    }

    /// <summary>
    /// Calcola l'hash code per il desktop virtuale
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, DeviceId);
    }
}

/// <summary>
/// Orientamento del display
/// </summary>
public enum DisplayOrientation
{
    /// <summary>
    /// Orientamento orizzontale (normale)
    /// </summary>
    Landscape = 0,

    /// <summary>
    /// Orientamento verticale (ruotato 90° in senso orario)
    /// </summary>
    Portrait = 1,

    /// <summary>
    /// Orientamento orizzontale rovesciato (ruotato 180°)
    /// </summary>
    LandscapeFlipped = 2,

    /// <summary>
    /// Orientamento verticale rovesciato (ruotato 90° in senso antiorario)
    /// </summary>
    PortraitFlipped = 3
}

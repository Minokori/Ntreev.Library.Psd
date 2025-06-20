namespace Ntreev.Library.Psd;

/// <summary>
/// 图层可视性
/// </summary>
[Flags]
public enum LayerFlags
    {
    Transparency = 1,

    Visible = 2,

    Obsolete = 4,

    Unknown0 = 8, // 1 for Photoshop 5.0 and later, tells if bit 4 has useful information;

    Unknown1 = 16, // pixel _data irrelevant to appearance of _document
    }

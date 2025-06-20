namespace Ntreev.Library.Psd;

internal partial class Channel
    {
    public byte[] Data { get; private set; } = [];
    public ChannelType Type { get; set; }
    public int Width { get; set; }

    public int Height { get; set; }

    public float Opacity { get; set; } = 1.0f;

    public long Size { get; set; }
    }


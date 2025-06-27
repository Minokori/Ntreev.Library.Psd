namespace Ntreev.Library.Psd;

internal partial class Channel
    {
    /// <summary>
    /// Data[行索引x * 行长度(宽度Width) + y] = 图片 (x,y) 处 的通道像素值
    /// </summary>
    public byte[] Data { get; private set; } = [];
    public ChannelType Type { get; set; }
    public int Width { get; set; }

    public int Height { get; set; }

    public float Opacity { get; set; } = 1.0f;

    public long StreamLength { get; set; }

    public int Depth { get; init; } = 1;

    public CompressionType CompressionType { get; set; }
    }


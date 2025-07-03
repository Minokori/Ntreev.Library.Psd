namespace Ntreev.Library.Psd;

internal partial class Channel(ChannelType type, int width, int height, int depth)
    {



    /// <summary>
    /// Data[行索引x * 行长度(宽度Width) + y] = 图片 (x,y) 处 的通道像素值
    /// </summary>
    /// <remarks>
    /// 懒加载在 <see cref="PsdLayer"/> 中实现. 访问 <see cref="PsdLayer.Channels"/> 时会从流中读取数据.<para/>
    /// 懒加载的方法在 <see cref="ReadImageStreamLazily(PsdBinaryReader, Newtonsoft.Json.Linq.JObject)"/>
    /// </remarks>
    public byte[] Data { get; private set; } = [];
    public ChannelType Type { get; init; } = type;
    public int Width { get; init; } = width;
    public int Height { get; init; } = height;
    public int Depth { get; init; } = depth;

    // meta data, for lazy loading data from stream

    /// <summary>
    /// size = Height
    /// </summary>
    /// <remarks>
    ///如果压缩代码为 1，则图像数据以通道中所有扫描行的字节计数 （LayerBottom-LayerTop） 开头，每个计数存储为双字节值。
    ///（**PSB** 每个计数都存储为一个四字节值。
    ///以下是 RLE 压缩数据，每条扫描线单独压缩。RLE 压缩与 Macintosh ROM 例程 PackBits 和 TIFF 标准使用的压缩算法相同。
    /// </remarks>
    public int[] RlePackLengths { get; set; } = [];




    /// <summary>
    /// 透明度, 0-1 (完全不透明)
    /// </summary>
    public float Opacity { get; set; } = 1.0f;
    public CompressionType CompressionType { get; set; }

    // TODO : try to make Data into a matrix
    /// <summary>
    /// shape = (h,w)
    /// </summary>
    }

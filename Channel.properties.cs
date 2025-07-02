using Newtonsoft.Json.Linq;

namespace Ntreev.Library.Psd;

internal partial class Channel
    {
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
    /// Data[行索引x * 行长度(宽度Width) + y] = 图片 (x,y) 处 的通道像素值
    /// </summary>
    /// <remarks>
    /// TODO 只有这个需要懒加载
    /// TODO 还没有实现懒加载, 现在是直接读取图像数据流到 Data 中, 不进行懒加载
    /// </remarks>
    public byte[] Data { get; private set; } = [];


    public ChannelType Type { get; set; }
    public int Width { get; set; }
    public int Height { get; init; }
    public float Opacity { get; set; } = 1.0f;
    public long StreamLength { get; set; }
    public int Depth { get; init; } = 1;
    public CompressionType CompressionType { get; set; }
    public JToken MetaInfo { get; internal set; }
    }

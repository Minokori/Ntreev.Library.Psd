namespace Ntreev.Library.Psd;

internal partial class Channel : IChannel
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
    /// 没有直接操作 reader的 属性, 可以用全局 reader 替代
    /// </summary>
    /// <param name="reader">私有 reader</param>
    /// <param name="compressionType"></param>
    public int[] ReadHeader(PsdBinaryReader reader, CompressionType compressionType)
        {
        if (compressionType != CompressionType.RLE)
            return [];

        var l = new int[Height];
        if (reader.Version == 1)
            {
            for (var i = 0; i < Height; i++)
                {
                l[i] = reader.ReadInt16();
                }
            }
        else
            {
            for (var i = 0; i < Height; i++)
                {
                l[i] = reader.ReadInt32();
                }
            }

        return l;
        }
    /// <summary>
    /// 没有直接操作 reader的 属性, 可以用全局 reader 替代
    /// </summary>
    /// <param name="reader">私有 reader</param>
    /// <param name="compressionType"></param>
    public void Read(PsdBinaryReader reader)
        {
        switch (CompressionType)
            {
            case CompressionType.Raw:
                PrivateReadData(reader, Depth, CompressionType, []);
                return;

            case CompressionType.RLE:
                PrivateReadData(reader, Depth, CompressionType, RlePackLengths);
                return;

            default:
                break;
            }

        }
    }

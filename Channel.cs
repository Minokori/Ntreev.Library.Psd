namespace Ntreev.Library.Psd;

internal partial class Channel : IChannel
    {






    /// <summary>
    /// 直接读取图像数据流到 Data 中, 不进行懒加载
    /// </summary>
    /// <param name="reader">私有 reader</param>
    /// <param name="compressionType"></param>
    public void ReadImageStream(PsdBinaryReader reader)
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

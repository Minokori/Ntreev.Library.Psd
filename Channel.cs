using Newtonsoft.Json.Linq;

namespace Ntreev.Library.Psd;

internal partial class Channel : IChannel
    {
    /// <summary>
    /// 直接读取图像数据流到 Data 中, 不进行懒加载
    /// </summary>
    /// <param name="reader">私有 reader</param>
    /// <param name="compressionType"></param>
    public void ReadImageStreamDirectly(PsdBinaryReader reader)
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

    public void ReadImageStreamLazy(PsdBinaryReader reader, JObject channelImageData)
        {
        var position = reader.Position;

        var compressionType = Enum.Parse<CompressionType>(channelImageData.ToValue<string>("CompressionType"));
        reader.Position = channelImageData.ToValue<long>("StartPosition");
        switch (compressionType)
            {

            case CompressionType.Raw:
                {
                PrivateReadData(reader, Depth, compressionType, []);
                break;
                }
            case CompressionType.RLE:
                {
                PrivateReadData(reader, Depth, compressionType, channelImageData.ToValue<int[]>("RlePackLengths"));
                break;

                }
            }

        reader.Position = position;
        }
    }

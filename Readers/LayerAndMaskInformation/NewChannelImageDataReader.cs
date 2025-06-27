
using Newtonsoft.Json.Linq;

namespace Ntreev.Library.Psd.Readers.LayerAndMaskInformation;

/// <summary>
/// 读: 通道压缩类型, 起止位置, 长度
/// </summary>
/// <param name="reader"></param>
/// <param name="length"></param>
/// <param name="record"></param>
internal class NewChannelImageDataReader(PsdBinaryReader reader, long length, JObject record) : ValueReader<JObject>(reader, length, record)
    {
    protected override JObject ReadValue()
        {
        var totalLength = StreamLength;
        var channelNum = ((JObject)UserData).SelectToken("ChannelCount").ToObject<short>();

        JObject data = [];
        for (var i = 0; i < channelNum; i++)
            {
            var compressionType = GlobalReader.ReadAsCompressionType();
            var dataStartPosition = GlobalReader.Position;

            }

        return [];
        }
    }

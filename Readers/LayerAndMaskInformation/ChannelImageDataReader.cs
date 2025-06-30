//Released under the MIT License.
//
//Copyright (c) 2015 Ntreev Soft co., Ltd.
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
//documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
//rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
//persons to whom the Software is furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
//Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
//OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using Newtonsoft.Json.Linq;

namespace Ntreev.Library.Psd.Readers.LayerAndMaskInformation;

/// <summary>
/// 提供 <see cref="ReadValue(PsdBinaryReader, object, out Ntreev.Library.Psd.Channel[])"/> 方法读取通道的数据
/// <para/>
/// 到 <paramref name="layer"/> 中
/// </summary>
/// <param name="reader"></param>
/// <param name="length">该 layer 所有 channel 的总长度</param>
/// <param name="layer">Channels</param>
internal class ChannelImageDataReader(PsdBinaryReader reader, long length, JObject layerRecord)
    : ValueReader<JArray>(reader, length, layerRecord)
//internal class ChannelImageDataReader(PsdBinaryReader reader, long length, PsdLayer layer) : ValueReader<Channel[]>(reader, length, layer)

    {
    protected override JArray ReadValue()
        {
        var records = UserData as JObject;
        var channels = records.InitChannels(GlobalReader.Depth);
        var count = records.SelectToken("ChannelCount")!.ToObject<int>();

        JArray ImageMetaData = [];
        foreach (var channel in channels)
        //for (var i = 0; i < count; i++)
            {
            JObject data = [];
            // 1. 读压缩类型
            var compressionType = GlobalReader.ReadAsCompressionType();
            data["CompressionType"] = Enum.GetName(compressionType);

            // 2. 读压缩长度
            var rlePackLength = compressionType == CompressionType.RLE ? GlobalReader.ReadAsChannelRlePackLengths(channel.Height) : [];
            data["RlePackLengths"] = new JArray(rlePackLength);

            // 3. 解压数据
            // 读取的长度:
            // RAW: depth * Width * Height
            // RLE: 每行的长度 Rle 相加
            data["StartPosition"] = GlobalReader.Position;
            var channelTotalLength =
                compressionType == CompressionType.Raw
                    ? PsdUtility.DepthToPitch(channel.Depth, channel.Width) * channel.Height
                    : rlePackLength.Sum();
            data["StreamLength"] = channelTotalLength;


            GlobalReader.Position += channelTotalLength;

            ImageMetaData.Add(data);
            }

        return ImageMetaData;
        }
    }

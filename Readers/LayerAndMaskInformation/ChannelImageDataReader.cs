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

using System.Diagnostics;
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
internal class ChannelImageDataReader(PsdBinaryReader reader, long length, PsdLayer layer)
    : ValueReader<JObject>(reader, length, layer)
//internal class ChannelImageDataReader(PsdBinaryReader reader, long length, PsdLayer layer) : ValueReader<Channel[]>(reader, length, layer)

    {
    protected override JObject ReadValue()
        {
        // TODO 需要 document 的 Depth
        // TODO 1. 变成一个 Channel的 info, 不要初始化Channel 包括 channel 的

        var layer = UserData as PsdLayer;
        var records = layer.Records;
        var postion = GlobalReader.Position;
        var channels = records.InitChannels(layer.Depth);
        (channels, var channelInfo) = PrivateReadValue(channels);
        Debug.WriteLine(channelInfo);
        return channelInfo;
        }

    /// <summary>
    /// TODO 删掉 reader 入参
    /// </summary>
    /// <param name="depth"></param>
    /// <param name="channels"></param>
    private Tuple<Channel[], JObject> PrivateReadValue(Channel[] channels)
        {
        JObject channelInfo = [];
        JArray compressionTypes = [];
        JArray rles = [];
        JArray dataStartPosition = [];
        JArray dataStreamLength = [];
        foreach (var channel in channels)
            {
            // 1. 读压缩类型
            var compressionType = GlobalReader.ReadAsCompressionType();
            channel.CompressionType = compressionType;
            compressionTypes.Add(Enum.GetName(compressionType));

            // 2. 读压缩长度
            var rlePackLength = channel.ReadHeader(GlobalReader, compressionType);
            channel.RlePackLengths = rlePackLength;
            rles.Add(rlePackLength);

            // 3. 解压数据
            // 读取的长度:
            // RAW: depth * Width * Height
            // RLE: 每行的长度 Rle 相加

            dataStartPosition.Add(GlobalReader.Position);
            var channelTotalLength =
                compressionType == CompressionType.Raw
                    ? PsdUtility.DepthToPitch(channel.Depth, channel.Width) * channel.Height
                    : rlePackLength.Sum();


            channel.Read(GlobalReader);

            //if (channelTotalLength % 2 != 0)
            //    {
            //    _ = GlobalReader.ReadByte();
            //    dataStreamLength.Add(channelTotalLength + 1);
            //    }

            }

        channelInfo["CompressionType"] = compressionTypes;
        channelInfo["RLEPackLength"] = rles;
        channelInfo["DataStartPosition"] = dataStartPosition;
        channelInfo["DataStreamLength"] = dataStreamLength;

        return new(channels, channelInfo);
        }
    }

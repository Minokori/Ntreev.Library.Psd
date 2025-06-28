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
/// layerCount + LayerRecords + ChannelImageData(暂时没有并进来)
/// </summary>
/// <param name="reader"></param>
internal class LayerInfoReader(PsdBinaryReader reader) : ValueReader<JObject>(reader, true, null)
    {
    protected override JObject ReadValue()
        {

        var layerCount = Math.Abs((int)GlobalReader.ReadInt16());

        Properties layerInfo = new() { ["LayerCount"] = layerCount };

        var layerRecords = new JArray();
        for (var i = 0; i < layerInfo["LayerCount"]!.ToObject<int>(); i++)
            {
            var record = new LayerRecordsReader(GlobalReader).Value;
            layerRecords.Add(record);
            }

        layerInfo["LayerRecord"] = layerRecords;


        var channelsImageDatas = new JArray();
        foreach (var item in layerRecords.Cast<JObject>())
            {
            var channelsTotalLength = item.ToValue<long[]>("ChannelDataLength")!.Sum();
            ChannelImageDataReader? channelReader = new(GlobalReader, channelsTotalLength, item);
            channelsImageDatas.Add(channelReader.Value);
            }


        layerInfo["ChannelsImageData"] = channelsImageDatas;
        return layerInfo;
        }
    }

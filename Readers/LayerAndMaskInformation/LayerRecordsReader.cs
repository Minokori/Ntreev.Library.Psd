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

internal class LayerRecordsReader(PsdBinaryReader reader) : ValueReader<LayerRecords>(reader, false, null), IDisposable
    {

    public void Dispose() { }

    protected override LayerRecords ReadValue()
        {
        LayerRecords records = new()
            {
            ["Top"] = GlobalReader.ReadInt32(),
            ["Left"] = GlobalReader.ReadInt32(),
            ["Bottom"] = GlobalReader.ReadInt32(),
            ["Right"] = GlobalReader.ReadInt32(),
            ["ChannelCount"] = GlobalReader.ReadUInt16(),
            };
        records.ValidateSize();

        records["ChannelID"] = new JArray();
        records["ChannelDataLength"] = new JArray();

        records.Channels = new Channel[records.ChannelCount];
        for (var i = 0; i < records.ChannelCount; i++)
            {
            records.Channels[i] = new();
            }

        for (var i = 0; i < records.ChannelCount; i++)
            {
            var id = GlobalReader.ReadAsChannelType();
            records.Channels[i].Type = id;
            (records["ChannelID"] as JArray)!.Add(Enum.GetName(id));
            var l = GlobalReader.ReadAsStreamLength();
            records.Channels[i].Size = l;
            (records["ChannelDataLength"] as JArray)!.Add(l);
            records.Channels[i].Width = records.Width;
            records.Channels[i].Height = records.Height;
            }

        _ = GlobalReader.VerifySignatureIs("8BIM");

        records["BlendMode"] = Enum.GetName(GlobalReader.ReadAsBlendMode());
        records["Opacity"] = GlobalReader.ReadByte();
        records["Clipping"] = GlobalReader.ReadBoolean();
        // TODO : LayerFlags
        records["Flags"] = (byte)GlobalReader.ReadAsLayerFlags();
        records["Filler"] = GlobalReader.ReadByte();

        StreamLength += 16 + 2 + (6 * records.ChannelCount) + 4 + 4 + 1 + 1 + 1 + 1 + 4 + GlobalReader.ReadInt32();


        records["Mask"] = new LayerMaskReader(GlobalReader).Value;
        records["BlendingRanges"] = new LayerBlendingRangesReader(GlobalReader).Value;
        var name = GlobalReader.ReadAsPascalString(4);


        var resources = new LayerResourceReader(GlobalReader, EndPosition - GlobalReader.Position).Value;
        records.AddRangeRecords(resources);
        //
        return records;
        }
    }

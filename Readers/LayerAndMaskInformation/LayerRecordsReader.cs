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

namespace Ntreev.Library.Psd.Readers.LayerAndMaskInformation;

internal class LayerRecordsReader : ValueReader<LayerRecords>
    {
    private LayerRecordsReader(PsdReader reader)
        : base(reader, false, null)
        {

        }

    public static LayerRecords Read(PsdReader reader)
        {
        LayerRecordsReader instance = new(reader);
        return instance.Value;
        }

    protected override LayerRecords ReadValue()
        {
        LayerRecords records = new()
            {
            Top = GlobalReader.ReadInt32(),
            Left = GlobalReader.ReadInt32(),
            Bottom = GlobalReader.ReadInt32(),
            Right = GlobalReader.ReadInt32()
            };
        records.ValidateSize();

        int channelCount = GlobalReader.ReadUInt16();

        records.ChannelCount = channelCount;

        for (var i = 0; i < channelCount; i++)
            {
            records.Channels[i].Type = GlobalReader.ReadChannelType();
            records.Channels[i].Size = GlobalReader.ReadLength();
            records.Channels[i].Width = records.Width;
            records.Channels[i].Height = records.Height;
            }

        GlobalReader.ValidateSignature();

        records.BlendMode = GlobalReader.ReadBlendMode();
        records.Opacity = GlobalReader.ReadByte();
        records.Clipping = GlobalReader.ReadBoolean();
        records.Flags = GlobalReader.ReadLayerFlags();
        records.Filter = GlobalReader.ReadByte();

        return records;
        }
    }


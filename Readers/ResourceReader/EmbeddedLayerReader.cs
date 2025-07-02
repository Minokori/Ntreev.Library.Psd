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
using Ntreev.Library.Psd.Attributes;
using Ntreev.Library.Psd.Structures;

namespace Ntreev.Library.Psd.Readers.ResourceReader;

[ResourceID("lnkE", DisplayName = "EmbeddedLayer")]
internal class EmbeddedReader(PsdBinaryReader reader, long length)
    : ValueReader<Properties>(reader, length, null)
    {
    protected override Properties ReadValue()
        {
        JArray embeddedLayerInfoList = [];

        while (GlobalReader.Position < EndPosition)
            {
            var endPosition = GlobalReader.ReadInt64().PadToFour() + GlobalReader.Position;
            JObject embeddedLayerInfo = new()
                {
                //'liFD' linked file data, 'liFE' linked file external or 'liFA' linked file alias
                ["Type"] = GlobalReader.VerifySignatureIs("liFD", "liFA", "liFE"),
                // Version ( = 1 to 7 )
                ["Version"] = GlobalReader.ReadInt32(),
                ["UniqueId"] = GlobalReader.ReadAsPascalString(),
                ["OriginalFileName"] = GlobalReader.ReadString(),
                ["FileType"] = GlobalReader.ReadAsType(),
                ["FileCreator"] = GlobalReader.ReadAsType(),
                };

            // length of data below
            _ = GlobalReader.ReadInt64();
            var fileOpenDescriptor = GlobalReader.ReadBoolean();
            if (fileOpenDescriptor)
                {
                _ = StructureReader.ReadDescriptor(GlobalReader);
                }
            // in document :If the type is 'liFE' then a linked file Descriptor is next.
            embeddedLayerInfo["DescriptorOfLinkedFile"] = StructureReader.ReadDescriptor(GlobalReader);

            if (embeddedLayerInfo.ToValue<int>("Version") > 3)
                {
                embeddedLayerInfo["Year"] = GlobalReader.ReadInt32();
                embeddedLayerInfo["Month"] = GlobalReader.ReadByte();
                embeddedLayerInfo["Day"] = GlobalReader.ReadByte();
                embeddedLayerInfo["Hour"] = GlobalReader.ReadByte();
                embeddedLayerInfo["Minute"] = GlobalReader.ReadByte();
                embeddedLayerInfo["Seconds"] = GlobalReader.ReadDouble();
                }

            embeddedLayerInfo["FileSize"] = GlobalReader.ReadInt64();
            if (embeddedLayerInfo.ToValue<int>("Version") >= 5)
                {
                embeddedLayerInfo["ChildDocumentId"] = GlobalReader.ReadString();
                }

            if (embeddedLayerInfo.ToValue<int>("Version") >= 6)
                {
                embeddedLayerInfo["AssetModTime"] = GlobalReader.ReadDouble();
                }

            if (embeddedLayerInfo.ToValue<int>("Version") >= 7)
                {
                embeddedLayerInfo["AssetLockedState"] = GlobalReader.ReadBoolean();
                }

            embeddedLayerInfoList.Add(embeddedLayerInfo);
            GlobalReader.Position = endPosition;
            }

        return new() { ["Info"] = embeddedLayerInfoList };
        }
    }

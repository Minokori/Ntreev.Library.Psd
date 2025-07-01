using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Attributes;
using Ntreev.Library.Psd.Readers.LayerAndMaskInformation;

namespace Ntreev.Library.Psd.Readers.ResourceReader;

[ResourceID("lnkD", "lnk2", "lnk3", DisplayName = "LinkedLayer")]
internal class LinkedLayerReader(PsdBinaryReader reader, long length)
    : ValueReader<Properties>(reader, length, null)
    {
    protected override Properties ReadValue()
        {
        Properties props = [];
        List<ILinkedLayer> linkedLayers = [];
        while (GlobalReader.Position < EndPosition)
            {
            // 注意: 先读取 **长度**, 使 GlobalReader.Position 移动后再计算 endPosition
            var endPosition = GlobalReader.ReadInt64().PadToFour() + GlobalReader.Position;
            JObject linkedLayerInfo = new()
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

            var lengthOfDataBelow = GlobalReader.ReadInt64();

            var fileOpenDescriptor = GlobalReader.ReadBoolean();

            DescriptorStructure? properties = null;
            if (fileOpenDescriptor)
                {
                properties = new DescriptorStructure(GlobalReader);
                }

            var isDocument = IsDocument(GlobalReader);
            LinkedDocumentReader documentReader = null;
            LinkedDocumentFileHeaderReader fileHeaderReader = null;
            if (lengthOfDataBelow > 0 && isDocument == true)
                {
                var position = GlobalReader.Position;
                documentReader = new LinkedDocumentReader(GlobalReader, lengthOfDataBelow);
                GlobalReader.Position = position;
                fileHeaderReader = new LinkedDocumentFileHeaderReader(GlobalReader, lengthOfDataBelow);
                }

            Debug.WriteLine("Linked Layer Info:\n" + linkedLayerInfo.ToString());
            var linkedLayer = new LinkedLayer(
                linkedLayerInfo,
                documentReader,
                fileHeaderReader
            );
            linkedLayers.Add(linkedLayer);
            GlobalReader.Position = endPosition;
            }

        props.AddLayers(linkedLayers);
        return props;
        }

    private static bool IsDocument(PsdBinaryReader reader)
        {
        var position = reader.Position;
        try
            {
            var signature = reader.ReadAsType();
            return signature == "8BPS";
            }
        finally
            {
            reader.Position = position;
            }
        }
    }
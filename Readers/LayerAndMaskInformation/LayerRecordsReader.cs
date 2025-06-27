using Newtonsoft.Json.Linq;

namespace Ntreev.Library.Psd.Readers.LayerAndMaskInformation;

internal class LayerRecordsReader(PsdBinaryReader reader) : ValueReader<JObject>(reader, false, null)
    {
    protected override JObject ReadValue()
        {
        JObject records = new()
            {
            ["Top"] = GlobalReader.ReadInt32(),
            ["Left"] = GlobalReader.ReadInt32(),
            ["Bottom"] = GlobalReader.ReadInt32(),
            ["Right"] = GlobalReader.ReadInt32(),
            ["ChannelCount"] = GlobalReader.ReadUInt16(),
            };

        records["ChannelID"] = new JArray();
        records["ChannelDataLength"] = new JArray();

        for (var i = 0; i < records.ToValue<int>("ChannelCount"); i++)
            {
            var id = GlobalReader.ReadAsChannelType();
            (records["ChannelID"] as JArray)!.Add(Enum.GetName(id));
            var l = GlobalReader.ReadAsStreamLength();
            (records["ChannelDataLength"] as JArray)!.Add(l);
            }

        _ = GlobalReader.VerifySignatureIs("8BIM");

        records["BlendMode"] = Enum.GetName(GlobalReader.ReadAsBlendMode());
        records["Opacity"] = GlobalReader.ReadByte();
        records["Clipping"] = GlobalReader.ReadBoolean();
        // TODO : LayerFlags
        records["Flags"] = (byte)GlobalReader.ReadAsLayerFlags();
        records["Filler"] = GlobalReader.ReadByte();

        // 计算 总长度(前面读取过的长度 + 可变长度部分的长度)
        // 可变部分: 5个, mask + blendingRanges + pascalName + resources
        StreamLength += 16 + 2 + (6 * records.ToValue<int>("ChannelCount")) + 4 + 4 + 1 + 1 + 1 + 1 + 4 + GlobalReader.ReadInt32();
        records["Mask"] = new LayerMaskReader(GlobalReader).Value;
        records["BlendingRanges"] = new LayerBlendingRangesReader(GlobalReader).Value;

        // BUG 乱码?
        records["PascalName"] = GlobalReader.ReadAsPascalString(4);

        var resources = new LayerResourceReader(GlobalReader, EndPosition - GlobalReader.Position).Value;
        records["Resources"] = resources;


        return records;
        }
    }

using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Attributes;

namespace Ntreev.Library.Psd.Readers.ImageResources;

/// <summary>
/// 切片资源格式
/// </summary>
/// <param name="reader"></param>
/// <param name="length"></param>
[ResourceID("1050", DisplayName = "Slices")]
internal class SlicesInfoReader(PsdBinaryReader reader, long length)
    : ValueReader<Properties>(reader, length, null)
    {
    protected override Properties ReadValue()
        {
        Properties props = [];

        var version = GlobalReader.ReadInt32();
        if (version == 6)  // Photoshop<=7.0
            {
            //Bounding rectangle for all of the slices: top, left, bottom, right of all the slices
            _ = GlobalReader.ReadInt32();
            _ = GlobalReader.ReadInt32();
            _ = GlobalReader.ReadInt32();
            _ = GlobalReader.ReadInt32();

            // Name of group of slices
            _ = GlobalReader.ReadString();

            // Number of slices to follow
            var count = GlobalReader.ReadInt32();


            // slice resource blocks
            var slices = new List<Properties>(count);
            for (var i = 0; i < count; i++)
                {
                slices.Add(ReadSliceResourceBlock(GlobalReader));
                }
            }

        // Photoshop>=CS
            {
            DescriptorStructure descriptor = new(GlobalReader);//as IProperties;

            var items = descriptor.SelectTokens("slices.Items[0]");

            var slices = new List<Properties>(items.Count());//items.Length
            foreach (var item in items)
                {
                slices.Add(ReadSliceInfo(item as Properties));
                }

            props["Items"] = new JArray(slices);
            }

        return props;
        }

    private static Properties ReadSliceResourceBlock(PsdBinaryReader reader)
        {
        var props = new Properties
            {
            ["ID"] = reader.ReadInt32(),
            ["GroupID"] = reader.ReadInt32(),
            };


        var origin = reader.ReadInt32();
        if (origin == 1)
            {
            var associatedLayerId = reader.ReadInt32();
            }

        props["Name"] = reader.ReadString();
        var type = reader.ReadInt32();

        props["Left"] = reader.ReadInt32();
        props["Top"] = reader.ReadInt32();
        props["Right"] = reader.ReadInt32();
        props["Bottom"] = reader.ReadInt32();

        props["Url"] = reader.ReadString();
        props["Target"] = reader.ReadString();
        props["Message"] = reader.ReadString();
        props["AltTag"] = reader.ReadString();

        var isCellTextHtml = reader.ReadBoolean();

        var cellText = reader.ReadString();

        props["Horizontal Alignment"] = reader.ReadInt32();
        props["Vertical Alignment"] = reader.ReadInt32();

        props["Alpha"] = reader.ReadByte();
        props["Red"] = reader.ReadByte();
        props["Green"] = reader.ReadByte();
        props["Blue"] = reader.ReadByte();

        return props;
        }

    private static Properties ReadSliceInfo(Properties properties)
        {
        var props = new Properties
            {
            ["ID"] = properties["sliceID"],
            ["GroupID"] = properties["groupID"],
            };
        if (properties.Contains("Nm") == true)
            props["Name"] = properties["Nm"];

        props["Left"] = properties.SelectToken("bounds.Left");
        props["Top"] = properties.SelectToken("bounds.Top");
        props["Right"] = properties.SelectToken("bounds.Rght");
        props["Bottom"] = properties.SelectToken("bounds.Btom");
        props["Url"] = properties["url"];
        props["Target"] = properties["null"];
        props["Message"] = properties["Msge"];
        props["AltTag"] = properties["altTag"];

        if (properties.Contains("bgColor") == true)
            {
            props["Alpha"] = properties.SelectToken("bgColor.alpha");
            props["Red"] = properties.SelectToken("bgColor.Rd");
            props["Green"] = properties.SelectToken("bgColor.Grn");
            props["Blue"] = properties.SelectToken("bgColor.Bl");
            }

        return props;
        }
    }

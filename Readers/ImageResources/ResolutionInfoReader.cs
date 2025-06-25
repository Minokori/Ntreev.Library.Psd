using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Attributes;
using Ntreev.Library.Psd.ReadersPrototype;

namespace Ntreev.Library.Psd.Readers.ImageResources;

/// <summary>
/// ResolutionInfo 结构体
/// </summary>
/// <param name="reader"></param>
/// <param name="length"></param>
[ResourceID("1005", DisplayName = "Resolution")]
internal class ResolutionInfoReader(PsdBinaryReader reader, long length) : ResourceReaderBase(reader, length)
    {
    protected override Properties ReadValue()
        {
        var props = new Properties(6)
            {
            ["HorizontalRes"] = GlobalReader.ReadInt16(),
            ["HorizontalResUnit"] = GlobalReader.ReadInt32(),
            ["WidthUnit"] = GlobalReader.ReadInt16(),
            ["VerticalRes"] = GlobalReader.ReadInt16(),
            ["VerticalResUnit"] = GlobalReader.ReadInt32(),
            ["HeightUnit"] = GlobalReader.ReadInt16(),
            };

        JObject json = new()
            {
            ["HorizontalRes"] = (short)props["HorizontalRes"],
            ["HorizontalResUnit"] = (int)props["HorizontalResUnit"],
            ["WidthUnit"] = (short)props["WidthUnit"],
            ["VerticalRes"] = (short)props["VerticalRes"],
            ["VerticalResUnit"] = (int)props["VerticalResUnit"],
            ["HeightUnit"] = (short)props["HeightUnit"]
            };
        Debug.WriteLine($"ResolutionInfo: {json}");
        return props;
        }
    }

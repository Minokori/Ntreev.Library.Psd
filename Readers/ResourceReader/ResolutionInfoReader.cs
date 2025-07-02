using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Attributes;

namespace Ntreev.Library.Psd.Readers.ImageResources;

/// <summary>
/// ResolutionInfo 结构体
/// </summary>
/// <param name="reader"></param>
/// <param name="length"></param>
[ResourceID("1005", DisplayName = "Resolution")]
internal class ResolutionInfoReader(PsdBinaryReader reader, long length)
    : ValueReader<JToken>(reader, length, null)
    {
    protected override JObject ReadValue()
        {
        var props = new JObject()
            {
            ["HorizontalRes"] = GlobalReader.ReadInt16(),
            ["HorizontalResUnit"] = GlobalReader.ReadInt32(),
            ["WidthUnit"] = GlobalReader.ReadInt16(),
            ["VerticalRes"] = GlobalReader.ReadInt16(),
            ["VerticalResUnit"] = GlobalReader.ReadInt32(),
            ["HeightUnit"] = GlobalReader.ReadInt16(),
            };
        return props;
        }
    }

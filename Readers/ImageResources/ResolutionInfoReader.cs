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
    protected override IProperties ReadValue()
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

        return props;
        }
    }

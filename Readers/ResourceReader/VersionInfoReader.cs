using Ntreev.Library.Psd.Attributes;
namespace Ntreev.Library.Psd.Readers.ImageResources;
/// <summary>
/// DescriptorVersion Info
/// </summary>
/// <param name="reader"></param>
/// <param name="length"></param>
[ResourceID("1057", DisplayName = "Version")]
internal class VersionInfoReader(PsdBinaryReader reader, long length) : ValueReader<Properties>(reader, length, null)
    {
    protected override Properties ReadValue()
        {
        var props = new Properties()
            {
            ["Version"] = GlobalReader.ReadInt32(),
            ["HasRealMergedData"] = GlobalReader.ReadBoolean(),
            ["WriterName"] = GlobalReader.ReadString(),
            ["ReaderName"] = GlobalReader.ReadString(),
            ["FileVersion"] = GlobalReader.ReadInt32()
            };

        return props;
        }
    }


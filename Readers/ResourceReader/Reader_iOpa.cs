using Ntreev.Library.Psd.Attributes;

namespace Ntreev.Library.Psd.Readers.ResourceReader;

[ResourceID("iOpa")]
internal class Reader_iOpa(PsdBinaryReader reader, long length)
    : ValueReader<Properties>(reader, length, null)
    {
    protected override Properties ReadValue()
        {
        var props = new Properties { ["Opacity"] = GlobalReader.ReadByte() };
        return props;
        }
    }

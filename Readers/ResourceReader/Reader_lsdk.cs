using Ntreev.Library.Psd.Attributes;

namespace Ntreev.Library.Psd.Readers.ResourceReader;

[ResourceID("lsdk")]
internal class Reader_lsdk(PsdBinaryReader reader, long length)
    : ValueReader<Properties>(reader, length, null)
    {
    protected override Properties ReadValue()
        {
        var props = new Properties { ["SectionType"] = GlobalReader.ReadInt32() };
        return props;
        }
    }

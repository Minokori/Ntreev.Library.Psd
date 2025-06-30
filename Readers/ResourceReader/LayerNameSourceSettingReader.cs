using Ntreev.Library.Psd.Attributes;

namespace Ntreev.Library.Psd.Readers.ResourceReader;

[ResourceID("lnsr", DisplayName = "LayerNameSourceSetting")]
internal class LayerNameSourceSettingReader(PsdBinaryReader reader, long length)
    : ValueReader<Properties>(reader, length, null)
    {
    protected override Properties ReadValue()
        {
        var props = new Properties { ["Name"] = GlobalReader.ReadAsAscii(4) };
        return props;
        }
    }

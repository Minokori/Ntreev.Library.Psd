using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Attributes;

namespace Ntreev.Library.Psd.Readers.ResourceReader;

[ResourceID("lnsr", DisplayName = "LayerNameSourceSetting")]
internal class LayerNameSourceSettingReader(PsdBinaryReader reader, long length)
    : ValueReader<JToken>(reader, length, null)
    {
    protected override JObject ReadValue()
        {
        var props = new JObject { ["Name"] = GlobalReader.ReadAsAscii(4) };
        return props;
        }
    }

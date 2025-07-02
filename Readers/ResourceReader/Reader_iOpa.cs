using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Attributes;

namespace Ntreev.Library.Psd.Readers.ResourceReader;

[ResourceID("iOpa")]
internal class Reader_iOpa(PsdBinaryReader reader, long length)
    : ValueReader<JToken>(reader, length, null)
    {
    protected override JObject ReadValue()
        {
        var props = new JObject { ["Opacity"] = GlobalReader.ReadByte() };
        return props;
        }
    }

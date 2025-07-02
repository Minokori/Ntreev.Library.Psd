using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Attributes;

namespace Ntreev.Library.Psd.Readers.ResourceReader;

[ResourceID("lsdk")]
internal class Reader_lsdk(PsdBinaryReader reader, long length)
    : ValueReader<JToken>(reader, length, null)
    {
    protected override JObject ReadValue()
        {
        var props = new JObject { ["SectionType"] = GlobalReader.ReadInt32() };
        return props;
        }
    }

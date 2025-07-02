using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Attributes;
using Ntreev.Library.Psd.Structures;

namespace Ntreev.Library.Psd.Readers.ResourceReader;

[ResourceID("lfx2", DisplayName = "ObjectBasedEffectsLayerInfo")]
internal class ObjectBasedEffectsLayerInfoReader(PsdBinaryReader reader, long length)
    : ValueReader<JToken>(reader, length, null)
    {
    protected override JObject ReadValue()
        {
        _ = GlobalReader.VerifyIntIs(0);
        return StructureReader.ReadDescriptor(GlobalReader);
        }
    }

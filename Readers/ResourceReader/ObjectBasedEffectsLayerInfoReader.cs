using Ntreev.Library.Psd.Attributes;
using Ntreev.Library.Psd.Structures;

namespace Ntreev.Library.Psd.Readers.ResourceReader;

[ResourceID("lfx2", DisplayName = "ObjectBasedEffectsLayerInfo")]
internal class ObjectBasedEffectsLayerInfoReader(PsdBinaryReader reader, long length)
    : ValueReader<Properties>(reader, length, null)
    {
    protected override Properties ReadValue()
        {
        _ = GlobalReader.VerifyIntIs(0);
        return Properties.FromJObject(StructureReader.ReadDescriptor(GlobalReader));
        }
    }

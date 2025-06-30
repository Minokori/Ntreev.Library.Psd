using Ntreev.Library.Psd.Attributes;
namespace Ntreev.Library.Psd.Readers.ResourceReader;

[ResourceID("SoLE", DisplayName = "SmartObjectLayerData")]
internal class SmartObjectLayerDataReader : Reader_SoLd
    {
    public SmartObjectLayerDataReader(PsdBinaryReader reader, long length)
        : base(reader, length)
        {

        }
    }


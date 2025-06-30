using Ntreev.Library.Psd.Attributes;

namespace Ntreev.Library.Psd.Readers.ResourceReader;

[ResourceID("lnkD", "lnk2", "lnk3", DisplayName = "LinkedLayer")]
internal class LinkedLayerReader(PsdBinaryReader reader, long length)
    : ValueReader<Properties>(reader, length, null)
    {
    protected override Properties ReadValue()
        {
        Properties props = [];
        List<ILinkedLayer> linkedLayers = [];
        while (GlobalReader.Position < EndPosition)
            {
            var r = new LayerAndMaskInformation.LinkedLayerReader(GlobalReader);
            linkedLayers.Add(r.Value);
            }

        props.AddLayers(linkedLayers);
        return props;
        }
    }

namespace Ntreev.Library.Psd.Readers;

internal class ImageResourcesSectionReader(PsdBinaryReader reader) : ValueReader<Properties>(reader, true, null)
    {
    protected override long InitStreamLength() => GlobalReader.ReadInt32();

    protected override Properties ReadValue()
        {
        Properties props = [];
        while (GlobalReader.Position < EndPosition)
            {
            _ = GlobalReader.VerifySignatureIs("8BIM"); // signature, 4 bytes
            var resourceID = GlobalReader.ReadInt16().ToString(); //Unique identifier for the resource. Image resource IDs contains a list of resource IDs used by Photoshop.
            _ = GlobalReader.ReadAsPascalString(2);//Name: Pascal string, padded to make the size even
            long length = GlobalReader.ReadInt32().PadToEven();// Actual size of resource data that follows (even)

            var resourceReader = ReaderCollector.CreateReader(resourceID, GlobalReader, length);
            if (resourceReader.Value.Count > 0)
                {
                props[ReaderCollector.GetDisplayName(resourceID)] = resourceReader.Value;
                }
            }

        return props;
        }
    }


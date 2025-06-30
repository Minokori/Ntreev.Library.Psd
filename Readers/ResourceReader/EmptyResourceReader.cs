namespace Ntreev.Library.Psd.Readers.ResourceReader;

internal class EmptyResourceReader(PsdBinaryReader reader, long length) : ValueReader<Properties>(reader, length, null)
    {
    protected override Properties ReadValue() => [];
    }


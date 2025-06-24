using Ntreev.Library.Psd.ReadersPrototype;

namespace Ntreev.Library.Psd.Readers;

internal class EmptyResourceReader(PsdBinaryReader reader, long length) : ResourceReaderBase(reader, length)
    {
    protected override Properties ReadValue() => [];
    }


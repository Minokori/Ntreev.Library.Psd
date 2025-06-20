namespace Ntreev.Library.Psd;

partial class PsdReader
    {

    public long Position
        {
        get => _reader.BaseStream.Position;
        set => _reader.BaseStream.Position = value;
        }

    public long Length => _reader.BaseStream.Length;

    public int Version
        {
        get => _version;
        set
            {
            if (value != 1 && value != 2)
                throw new InvalidFormatException();

            _version = value;
            }
        }

    public PsdUriResolver Resolver
        {
        get => _resolver;
        }

    public Stream Stream
        {
        get => _stream;
        }

    public Uri Uri
        {
        get => _uri;
        }
    }



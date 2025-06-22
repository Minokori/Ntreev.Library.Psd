namespace Ntreev.Library.Psd;

internal partial class Channel : IChannel
    {
    private byte[] _data = [];
    private ChannelType _type;
    private int[] _rlePackLengths = [];

    public void ReadHeader(PsdBinaryReader reader, CompressionType compressionType)
        {
        if (compressionType != CompressionType.RLE)
            return;

        this._rlePackLengths = new int[Height];
        if (reader.Version == 1)
            {
            for (var i = 0; i < Height; i++)
                {
                this._rlePackLengths[i] = reader.ReadInt16();
                }
            }
        else
            {
            for (var i = 0; i < Height; i++)
                {
                this._rlePackLengths[i] = reader.ReadInt32();
                }
            }
        }

    public void Read(PsdBinaryReader reader, int bpp, CompressionType compressionType)
        {
        switch (compressionType)
            {
            case CompressionType.Raw:
                PrivateReadData(reader, bpp, compressionType, []);
                return;

            case CompressionType.RLE:
                PrivateReadData(reader, bpp, compressionType, this._rlePackLengths);
                return;

            default:
                break;
            }
        }
    }

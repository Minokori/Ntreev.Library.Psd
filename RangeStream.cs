namespace Ntreev.Library.Psd;

internal class RangeStream(Stream stream, long position, long length) : Stream
    {
    private readonly Stream stream = stream;
    private readonly long position = position;
    private readonly long length = length;

    public override bool CanRead => true;

    public override bool CanSeek => true;

    public override bool CanWrite => false;

    public override void Flush() { }

    public override long Length => length;

    public override long Position
        {
        get => stream.Position - position;
        set => stream.Position = position + value;
        }

    public override int Read(byte[] buffer, int offset, int count) =>
        stream.Read(buffer, offset, count);

    public override long Seek(long offset, SeekOrigin origin)
        {
        return origin == SeekOrigin.Current
            ? stream.Seek(offset, origin) - position
            : stream.Seek(position + offset, origin) - position;
        }

    public override void SetLength(long value) => throw new NotImplementedException();

    public override void Write(byte[] buffer, int offset, int count) =>
        throw new NotImplementedException();
    }

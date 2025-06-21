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

    public override long Length => this.length;

    public override long Position
        {
        get => this.stream.Position - this.position;
        set => this.stream.Position = this.position + value;
        }

    public override int Read(byte[] buffer, int offset, int count) =>
        this.stream.Read(buffer, offset, count);

    public override long Seek(long offset, SeekOrigin origin)
        {
        return origin == SeekOrigin.Current
            ? this.stream.Seek(offset, origin) - this.position
            : this.stream.Seek(this.position + offset, origin) - this.position;
        }

    public override void SetLength(long value) => throw new NotImplementedException();

    public override void Write(byte[] buffer, int offset, int count) =>
        throw new NotImplementedException();
    }

namespace Ntreev.Library.Psd;

partial class ValueReader<T>
    {

    public T Value
        {
        get
            {
            if (this.isRead == false && this.length > 0)
                {
                long position = reader.Position;
                int version = reader.Version;
                this.Refresh();
                reader.Position = position;
                reader.Version = version;
                }
            return this.value;
            }
        }

    public long Length => this.length;

    public long Position => this.position;

    public long EndPosition => this.position + this.length;

    }


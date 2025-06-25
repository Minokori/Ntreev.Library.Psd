namespace Ntreev.Library.Psd;
internal partial class LayerRecords
    {
    public int Top { get; set; }
    public int Left { get; set; }
    public int Bottom { get; set; }
    public int Right { get; set; }
    public int Width => Right - Left;
    public int Height => Bottom - Top;
    public int ChannelCount
        {
        get => Channels == null ? 0 : Channels.Length;
        set
            {
            if (value > 0x38)
                {
                throw new Exception(string.Format("Too many channels : {0}", value));
                }

            Channels = new Channel[value];
            for (var i = 0; i < value; i++)
                {
                Channels[i] = new Channel();
                }
            }
        }
    public Channel[] Channels { get; private set; }
    public BlendMode BlendMode { get; set; }
    /// <summary>
    /// 0 = 透明 ...255 = 不透明
    /// </summary>
    public byte Opacity { get; set; }
    /// <summary>
    /// 0 = base, 1 = non-base
    /// </summary>
    public bool Clipping { get; set; }
    public LayerFlags Flags { get; set; }

    public int Filler { get; set; }

    public long ChannelSize => Channels.Select(item => item.Size).Aggregate((v, n) => v + n);

    public SectionType SectionType { get; private set; }

    public Guid PlacedID { get; private set; }

    public string Name => name;

    public LayerMask Mask { get; set; }

    public LayerBlendingRanges BlendingRanges { get; set; }

    // TODO
    public Properties Resources { get; private set; }

    public int Version => this.version;
    }

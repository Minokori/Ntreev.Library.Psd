namespace Ntreev.Library.Psd;
internal partial class LayerRecords
    {

    // defined in document
    public int Top => SelectToken("Top")!.ToObject<int>()!;
    public int Left => SelectToken("Left")!.ToObject<int>()!;
    public int Bottom => SelectToken("Bottom")!.ToObject<int>()!;
    public int Right => SelectToken("Right")!.ToObject<int>()!;
    public int ChannelCount
        {
        get
            {
            var i = SelectToken("ChannelCount")?.ToObject<int>();
            return i is null ? 0 : i.Value;
            }
        set
            {
            if (value > 0x38)
                {
                throw new Exception(string.Format("Too many channels : {0}", value));
                }

            this["ChannelCount"] = value;
            Channels = new Channel[value];
            for (var i = 0; i < value; i++)
                {
                Channels[i] = new Channel();
                }
            }
        }


    // channel information, id + length of channel data
    // 直接移到 Channels 属性中了
    public BlendMode BlendMode => Enum.Parse<BlendMode>(SelectToken("BlendMode")!.ToObject<string>()!);

    /// <summary>
    /// 0 = 透明 ...255 = 不透明
    /// </summary>
    public byte Opacity => SelectToken("Opacity")!.ToObject<byte>()!;
    /// <summary>
    /// 0 = base, 1 = non-base
    /// </summary>
    public bool Clipping => SelectToken("Clipping")!.ToObject<bool>()!;
    public LayerFlags Flags => Enum.Parse<LayerFlags>(SelectToken("Flags")!.ToObject<string>()!);
    public int Filler => SelectToken("Filler")!.ToObject<int>()!;

    public LayerMask Mask => this["Mask"]!.ToObject<LayerMask>()!;


    public int Width => Right - Left;
    public int Height => Bottom - Top;

    public Channel[] Channels { get; set; }


    public long ChannelSize => Channels.Select(item => item.Size).Aggregate((v, n) => v + n);

    public SectionType SectionType { get; private set; }

    public Guid PlacedID { get; private set; }

    public string Name { get; private set; }



    public LayerBlendingRanges BlendingRanges { get; set; }

    public Properties Resources { get; private set; }

    public int Version { get; private set; }
    }

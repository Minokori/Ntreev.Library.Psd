using Newtonsoft.Json.Linq;

namespace Ntreev.Library.Psd;

internal partial class PsdLayer
    {
    // TODO 把 PSDLayer 变成组合, 让属性从组合中获取

    #region 来自record的属性
    public SectionType SectionType
        {
        get
            {
            var type = Records.ToValue<string>("Resources.lsct.SectionType", "Resources.lsdk.SectionType");
            return string.IsNullOrEmpty(type) ? SectionType.Normal : Enum.Parse<SectionType>(type);
            }
        }
    public string Name => Records.ToValue<string>("Resources.UnicodeLayerName.Name");

    public bool IsVisible => (Enum.Parse<LayerFlags>(Records.ToValue<string>("Flags")) & LayerFlags.Visible) != LayerFlags.Visible;

    public float Opacity => Records.ToValue<float>("Opacity") / 255f;
    public bool IsClipping => Records.ToValue<bool>("Clipping");

    public BlendMode BlendMode => Enum.Parse<BlendMode>(Records.ToValue<string>("BlendMode"));

    #endregion


    #region 来自record的属性


    #endregion
    public Channel[] Channels { get; init; }// _channelsReader.Value;




    public int Left { get; private set; }

    public int Top { get; private set; }

    public int Right { get; private set; }

    public int Bottom { get; private set; }

    public int Width => Right - Left;

    public int Height => Bottom - Top;

    public int Depth => GlobalReader.Depth;


    public PsdLayer Parent { get; set; }

    public PsdLayer[] Childs
        {
        get => (field) ?? _emptyChilds;
        set;
        } = [];

    public Properties Resources => Records.ToValue<Properties>("Resources");

    public PsdDocument Document { get; init; }

    public JObject Records { get; set; }
    public JArray ChannelImageData { get; private set; }

    public ILinkedLayer LinkedLayer
        {
        get
            {
            var placeID = new Guid(Records.ToValue<string>("PlacedID"));

            if (placeID == Guid.Empty)
                return null;

            field ??= Document.LinkedLayers.Where(i => i.ID == placeID && i.HasDocument).FirstOrDefault();
            return field;
            }
        }

    public bool HasImage => SectionType == SectionType.Normal && Width != 0 && Height != 0;

    public bool HasMask => Records.Contains("Mask");
    #region IPsdLayer

    IPsdLayer IPsdLayer.Parent => Parent == null ? Document : Parent;

    IChannel[] IImageSource.Channels => Channels;

    IPsdLayer[] IPsdLayer.Childs => Childs;

    #endregion
    }


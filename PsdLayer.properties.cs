using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Interfaces;

namespace Ntreev.Library.Psd;

internal partial class PsdLayer
    {
    // TODO 把 PSDLayer 变成组合, 让属性从组合中获取

    #region 来自record的属性
    public SectionType SectionType
        {
        get
            {
            var type = Records.ToValue<string>(
                "Resources.SectionDividerSetting.SectionType",
                "Resources.lsdk.SectionType"
            );
            return string.IsNullOrEmpty(type) ? SectionType.Normal : Enum.Parse<SectionType>(type);
            }
        }
    public string Name => Records.ToValue<string>("Resources.UnicodeLayerName.Name");

    public bool IsVisible =>
        (Enum.Parse<LayerFlags>(Records.ToValue<string>("Flags")) & LayerFlags.Visible)
        != LayerFlags.Visible;

    public float Opacity => Records.ToValue<float>("Opacity") / 255f;
    public bool IsClipping => Records.ToValue<bool>("Clipping");

    public BlendMode BlendMode => Enum.Parse<BlendMode>(Records.ToValue<string>("BlendMode"));

    #endregion


    public Channel[] Channels
        {
        get
            {
            foreach (var (channel, channelImageData) in field.Zip(ChannelImageData.Cast<JObject>()))
                {
                if (channel.Data.Length != 0)
                    continue;
                channel.ReadImageStreamLazily(GlobalReader, channelImageData);
                }

            return field;
            }
        init;
        }

    public int Left
        {
        get
            {
            if (field < 0)
                field = Records.ToValue<int>("Left");
            return field;
            }
        private set;
        } = -1;

    public int Top
        {
        get
            {
            if (field < 0)
                field = Records.ToValue<int>("Top");
            return field;
            }
        private set;
        } = -1;

    public int Right
        {
        get
            {
            if (field < 0)
                field = Records.ToValue<int>("Right");
            return field;
            }
        private set;
        } = -1;

    public int Bottom
        {
        get
            {
            if (field < 0)
                field = Records.ToValue<int>("Bottom");
            return field;
            }
        private set;
        } = -1;

    public int Width => Right - Left;

    public int Height => Bottom - Top;

    public int Depth => GlobalReader.Depth;

    public PsdLayer? Parent { get; set; }

    public PsdLayer[] Childs { get; set; } = [];

    public JObject Resources => Records.ToValue<JObject>("Resources"); //TODO UnalbleTOCast

    public PsdDocument Document { get; init; }

    public JObject Records { get; set; }
    public JArray ChannelImageData { get; private set; }

    public ILinkedLayer LinkedLayer
        {
        get
            {
            //Resources.SmartObjectLayerData.Idnt
            var guidString = Records.ToValue<string>("Resources.PlacedLayer.UniqueId");

            if (guidString is null)
                return null;
            var placeID = new Guid(guidString);

            field ??= Document
                .LinkedLayers.Where(i => i.ID == placeID && i.HasDocument)
                .FirstOrDefault();
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

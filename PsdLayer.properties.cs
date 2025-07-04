using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Interfaces;

namespace Ntreev.Library.Psd;

internal partial class PsdLayer
    {
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
    public string Name => Records.ToValue<string>("Resources.UnicodeLayerName.Name") ?? "";

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

    public JObject Resources => Records.ToValue<JObject>("Resources");

    public PsdDocument Document { get; init; }

    public JObject Records { get; set; }
    public JArray ChannelImageData { get; private set; }

    public ILinkedLayer? LinkedLayer
        {
        get
            {
            //var guidString = Records.ToValue<string>("Resources.PlacedLayer.UniqueId");
            var guidString = Records.ToValue<string>("Resources.SmartObjectLayerData.Idnt");
            if (guidString is null)
                return null;
            var placeID = new Guid(guidString);

            field ??= Document
                .LinkedLayers.Where(i => i.ID == placeID && i.HasDocument)
                .FirstOrDefault();
            return field;
            }
        }


    /// <summary>
    /// 是否有图像数据, 一般情况下为 <see cref="true"/>, 但 特殊的图层类型如 <see cref="SectionType.Divider"/> 可能没有图像数据
    /// </summary>
    public bool HasImage => SectionType == SectionType.Normal && Width != 0 && Height != 0;

    public bool HasMask => Records.Contains("Mask");

    #region IPsdLayer

    IPsdLayer IPsdLayer.Parent => Parent == null ? Document : Parent;

    IChannel[] IPsdLayer.Channels => Channels;

    IPsdLayer[] IPsdLayer.Childs => Childs;

    #endregion
    }

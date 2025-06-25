namespace Ntreev.Library.Psd;

internal partial class PsdLayer
    {

    public Channel[] Channels => _channelsReader.Value;

    public SectionType SectionType => Records.SectionType;

    public string Name => Records.Name;

    public bool IsVisible => (Records.Flags & LayerFlags.Visible) != LayerFlags.Visible;

    public float Opacity => Records.Opacity / 255f;

    public int Left { get; private set; }

    public int Top { get; private set; }

    public int Right { get; private set; }

    public int Bottom { get; private set; }

    public int Width => Right - Left;

    public int Height => Bottom - Top;

    public int Depth => Document.FileHeaderSection.Depth;

    public bool IsClipping => Records.Clipping;

    public BlendMode BlendMode => Records.BlendMode;

    public PsdLayer Parent { get; set; }

    public PsdLayer[] Childs
        {
        get => (field) ?? _emptyChilds;
        set;
        } = [];

    public Properties Resources => Records.Resources;

    public PsdDocument Document { get; }

    public LayerRecords Records { get; }

    public ILinkedLayer LinkedLayer
        {
        get
            {
            var placeID = Records.PlacedID;

            if (placeID == Guid.Empty)
                return null;

            field ??= Document.LinkedLayers.Where(i => i.ID == placeID && i.HasDocument).FirstOrDefault();
            return field;
            }
        }

    public bool HasImage => Records.SectionType == SectionType.Normal && Width != 0 && Height != 0;

    public bool HasMask => Records.Mask != null;
    #region IPsdLayer

    IPsdLayer IPsdLayer.Parent => Parent == null ? Document : Parent;

    IChannel[] IImageSource.Channels => _channelsReader.Value;

    IPsdLayer[] IPsdLayer.Childs => Childs;

    #endregion
    }


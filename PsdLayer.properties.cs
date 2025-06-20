namespace Ntreev.Library.Psd;

internal partial class PsdLayer
    {

    public Channel[] Channels => _channelsReader.Value;

    public SectionType SectionType => _records.SectionType;

    public string Name => _records.Name;

    public bool IsVisible => (_records.Flags & LayerFlags.Visible) != LayerFlags.Visible;

    public float Opacity => ((float)_records.Opacity) / 255f;

    public int Left => _left;

    public int Top => _top;

    public int Right => _right;

    public int Bottom => _bottom;

    public int Width => _right - _left;

    public int Height => _bottom - _top;

    public int Depth => _document.FileHeaderSection.Depth;

    public bool IsClipping => _records.Clipping;

    public BlendMode BlendMode => _records.BlendMode;

    public PsdLayer Parent
        {
        get => _parent;
        set { _parent = value; }
        }

    public PsdLayer[] Childs
        {
        get
            {
            if (_childs == null)
                return _emptyChilds;
            return _childs;
            }
        set => _childs = value;
        }

    public IProperties Resources => _records.Resources;

    public PsdDocument Document => _document;

    public LayerRecords Records => _records;

    public ILinkedLayer LinkedLayer
        {
        get
            {
            Guid placeID = _records.PlacedID;

            if (placeID == Guid.Empty)
                return null;

            _linkedLayer ??= _document.LinkedLayers.Where(i => i.ID == placeID && i.HasDocument).FirstOrDefault();
            return _linkedLayer;
            }
        }

    public bool HasImage
        {
        get
            {
            if (_records.SectionType != SectionType.Normal)
                return false;
            if (Width == 0 || Height == 0)
                return false;
            return true;
            }
        }

    public bool HasMask => _records.Mask != null;
    #region IPsdLayer

    IPsdLayer IPsdLayer.Parent
        {
        get
            {
            if (_parent == null)
                return _document;
            return _parent;
            }
        }

    IChannel[] IImageSource.Channels
        {
        get { return _channelsReader.Value; }
        }

    IPsdLayer[] IPsdLayer.Childs
        {
        get { return Childs; }
        }

    #endregion
    }


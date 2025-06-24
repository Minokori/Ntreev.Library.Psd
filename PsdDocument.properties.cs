namespace Ntreev.Library.Psd;

public partial class PsdDocument
    {
    public FileHeaderSection FileHeaderSection => fileHeaderSection.Value;

    public byte[] ColorModeData => colorModeDataSection.Value;

    public int Width => fileHeaderSection.Value.Width;

    public int Height => fileHeaderSection.Value.Height;

    public int Depth => fileHeaderSection.Value.Depth;

    public IPsdLayer[] Childs => layerAndMaskSection.Value.Layers;

    public IEnumerable<ILinkedLayer> LinkedLayers => layerAndMaskSection.Value.LinkedLayers;

    public IEnumerable<KeyValuePair<string, object>> Resources => layerAndMaskSection.Value.Resources;

    public IEnumerable<KeyValuePair<string, object>> ImageResources => imageResourcesSection;

    public bool HasImage =>
        imageResourcesSection.Contains("Version") != false
        && imageResourcesSection.ToBoolean("Version", "HasCompatibilityImage");

    #region IPsdLayer

    IPsdLayer IPsdLayer.Parent => null;

    bool IPsdLayer.IsClipping => false;

    PsdDocument IPsdLayer.Document => this;

    ILinkedLayer IPsdLayer.LinkedLayer => null;

    string IPsdLayer.Name => "Document";

    int IPsdLayer.Left => 0;

    int IPsdLayer.Top => 0;

    int IPsdLayer.Right => this.Width;

    int IPsdLayer.Bottom => this.Height;

    BlendMode IPsdLayer.BlendMode => BlendMode.Normal;

    IChannel[] IImageSource.Channels => this.imageDataSection.Value;

    // TODO This makes MergeChannels on PsdDocument class no opacity
    float IImageSource.Opacity => 1.0f;

    bool IImageSource.HasMask => this.FileHeaderSection.NumberOfChannels > 4;
    #endregion
    }

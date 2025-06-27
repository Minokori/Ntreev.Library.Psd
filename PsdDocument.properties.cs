namespace Ntreev.Library.Psd;

public partial class PsdDocument
    {
    public FileHeaderSection FileHeaderSection { get; private set; }

    public Properties ColorModeDataSection { get; private set; }

    public byte[] ColorModeData => ColorModeDataSection["ColorMode"]!.ToObject<byte[]>()!;

    public int Width => FileHeaderSection.Width;

    public int Height => FileHeaderSection.Height;

    public int Depth => FileHeaderSection.Depth;

    public IPsdLayer[] Childs => layerAndMaskSection.Value.Layers;

    public IEnumerable<ILinkedLayer> LinkedLayers => layerAndMaskSection.Value.LinkedLayers;

    public Properties Resources => layerAndMaskSection.Value.Resources;

    // TODO
    public Properties ImageResources { get; private set; }

    //TODO
    public bool HasImage =>
        ImageResources.Contains("Version") != false
        && ImageResources.SelectToken("Version.HasRealMergedData").ToObject<bool>();

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

    IChannel[] IImageSource.Channels => imageDataSection.Value;

    // TODO This makes MergeChannels on PsdDocument class no opacity
    float IImageSource.Opacity => 1.0f;

    bool IImageSource.HasMask => this.FileHeaderSection.NumberOfChannels > 4;
    #endregion
    }

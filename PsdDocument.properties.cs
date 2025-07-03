using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Interfaces;
using Ntreev.Library.Psd.Sections;

namespace Ntreev.Library.Psd;

public partial class PsdDocument
    {
    // Stream Reader
    internal PsdBinaryReader BinaryReader { get; init; }

    // sections
    public FileHeaderSection FileHeaderSection { get; private set; }

    public JObject ColorModeDataSection { get; private set; }

    internal LayerAndMaskInformationSection LayerAndMaskSection { get; private set; }
    internal ImageDataSection ImageDataSection { get; private set; }

    public byte[] ColorModeData => ColorModeDataSection["ColorMode"]!.ToObject<byte[]>()!;

    public int Width => FileHeaderSection.Width;

    public int Height => FileHeaderSection.Height;

    public int Depth => FileHeaderSection.Depth;

    public IPsdLayer[] Childs => LayerAndMaskSection.Layers; //LayerAndMaskSection.Value.Layers;

    public IEnumerable<ILinkedLayer> LinkedLayers => LayerAndMaskSection.LinkedLayers; //LayerAndMaskSection.Value.LinkedLayers;

    public JObject Resources => LayerAndMaskSection.AdditionalLayerInformation; //LayerAndMaskSection.Value.AdditionalLayerInfomation;

    // TODO
    public JObject ImageResourcesSection { get; private set; }

    //TODO
    public bool HasImage =>
        ImageResourcesSection.Contains("Version") != false
        && ImageResourcesSection.SelectToken("Version.HasRealMergedData").ToObject<bool>();

    #region IPsdLayer

    IPsdLayer IPsdLayer.Parent => null;

    bool IPsdLayer.IsClipping => false;

    PsdDocument IPsdLayer.Document => this;

    ILinkedLayer IPsdLayer.LinkedLayer => null;

    string IPsdLayer.Name => "Document";

    int IPsdLayer.Left => 0;

    int IPsdLayer.Top => 0;

    int IPsdLayer.Right => Width;

    int IPsdLayer.Bottom => Height;

    BlendMode IPsdLayer.BlendMode => BlendMode.Normal;

    IChannel[] IPsdLayer.Channels => ImageDataSection.Channels;

    // TODO This makes MergeChannels on PsdDocument class no opacity
    float IPsdLayer.Opacity => 1.0f;

    bool IPsdLayer.HasMask => FileHeaderSection.NumberOfChannels > 4;

    public Uri Uri => BinaryReader.Uri;
    #endregion
    }

namespace Ntreev.Library.Psd;

public partial class PsdDocument
    {


    public FileHeaderSection FileHeaderSection
        {
        get { return this.fileHeaderSection.Value; }
        }

    public byte[] ColorModeData
        {
        get { return this.colorModeDataSection.Value; }
        }

    public int Width
        {
        get { return this.fileHeaderSection.Value.Width; }
        }

    public int Height
        {
        get { return this.fileHeaderSection.Value.Height; }
        }

    public int Depth
        {
        get { return this.fileHeaderSection.Value.Depth; }
        }

    public IPsdLayer[] Childs
        {
        get { return this.layerAndMaskSection.Value.Layers; }
        }

    public IEnumerable<ILinkedLayer> LinkedLayers
        {
        get { return this.layerAndMaskSection.Value.LinkedLayers; }
        }

    public IProperties Resources
        {
        get { return this.layerAndMaskSection.Value.Resources; }
        }

    public IProperties ImageResources
        {
        get { return this.imageResourcesSection; }
        }

    public bool HasImage
        {
        get
            {
            if (this.imageResourcesSection.Contains("Version") == false)
                return false;
            return this.imageResourcesSection.ToBoolean("Version", "HasCompatibilityImage");
            }
        }

    #region IPsdLayer

    IPsdLayer IPsdLayer.Parent
        {
        get { return null; }
        }

    bool IPsdLayer.IsClipping
        {
        get { return false; }
        }

    PsdDocument IPsdLayer.Document
        {
        get { return this; }
        }

    ILinkedLayer IPsdLayer.LinkedLayer
        {
        get { return null; }
        }

    string IPsdLayer.Name
        {
        get { return "Document"; }
        }

    int IPsdLayer.Left
        {
        get { return 0; }
        }

    int IPsdLayer.Top
        {
        get { return 0; }
        }

    int IPsdLayer.Right
        {
        get { return this.Width; }
        }

    int IPsdLayer.Bottom
        {
        get { return this.Height; }
        }

    BlendMode IPsdLayer.BlendMode
        {
        get { return BlendMode.Normal; }
        }

    IChannel[] IImageSource.Channels
        {
        get { return this.imageDataSection.Value; }
        }


    // TODO This makes MergeChannels on PsdDocument class no opacity
    float IImageSource.Opacity
        {
        get { return 1.0f; }
        }

    bool IImageSource.HasMask
        {
        get { return this.FileHeaderSection.NumberOfChannels > 4; }
        }
    #endregion
    }


using Ntreev.Library.Psd.Readers;

namespace Ntreev.Library.Psd;

public partial class PsdDocument : IPsdLayer, IDisposable
    {
    private FileHeaderSectionReader? fileHeaderSection;
    private ColorModeDataSectionReader? colorModeDataSection;
    private ImageResourcesSectionReader? imageResourcesSection;
    private LayerAndMaskInformationSectionReader? layerAndMaskSection;
    private ImageDataSectionReader? imageDataSection;
    internal PsdReader BinaryReader { get; init; }

    public void Dispose()
        {
        if (BinaryReader == null)
            return;

        BinaryReader.Dispose();
        OnDisposed(EventArgs.Empty);

        // Suppress finalization to comply with CA1816
        GC.SuppressFinalize(this);
        }

    public event EventHandler? Disposed;

    protected virtual void OnDisposed(EventArgs e) => Disposed?.Invoke(this, e);

    internal void InitSections()
        {

        fileHeaderSection = new FileHeaderSectionReader(BinaryReader);
        colorModeDataSection = new ColorModeDataSectionReader(BinaryReader);
        imageResourcesSection = new ImageResourcesSectionReader(BinaryReader);
        layerAndMaskSection = new LayerAndMaskInformationSectionReader(BinaryReader, this);
        imageDataSection = new ImageDataSectionReader(BinaryReader, this);
        }
    }

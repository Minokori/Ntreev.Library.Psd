using System.Diagnostics;
using Ntreev.Library.Psd.Readers;

namespace Ntreev.Library.Psd;

public partial class PsdDocument : IPsdLayer, IDisposable
    {
    private LayerAndMaskInformationSectionReader? layerAndMaskSection;
    private ImageDataSectionReader? imageDataSection;
    internal PsdBinaryReader BinaryReader { get; init; }

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
        FileHeaderSection = new FileHeaderSectionReader(BinaryReader).Value;

        ColorModeDataSection = new()
            {
            ["ColorModeData"] = new ColorModeDataSectionReader(BinaryReader).Value
            };
        ImageResources = new ImageResourcesSectionReader(BinaryReader).Value;

        layerAndMaskSection = new LayerAndMaskInformationSectionReader(BinaryReader, this);
        imageDataSection = new ImageDataSectionReader(BinaryReader, this);

        Debug.WriteLine(ImageResources);
        }
    }

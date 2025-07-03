using Ntreev.Library.Psd.Interfaces;
using Ntreev.Library.Psd.Readers;

namespace Ntreev.Library.Psd;

public partial class PsdDocument : IPsdLayer, IDisposable
    {
    //private LayerAndMaskInformationSectionReader? LayerAndMaskSection;

    public void Dispose()
        {
        BinaryReader?.Dispose();
        OnDisposed(EventArgs.Empty);

        GC.SuppressFinalize(this);
        }

    public event EventHandler? Disposed;

    protected virtual void OnDisposed(EventArgs e) => Disposed?.Invoke(this, e);

    internal void InitSections()
        {
        FileHeaderSection = new FileHeaderSectionReader(BinaryReader).Value;

        BinaryReader.Depth = FileHeaderSection.Depth;

        ColorModeDataSection = new()
            {
            [nameof(ColorModeData)] = new ColorModeDataSectionReader(BinaryReader).Value,
            };
        ImageResourcesSection = new ImageResourcesSectionReader(BinaryReader).Value;

        LayerAndMaskSection = new LayerAndMaskInformationSectionReader(BinaryReader, this).Value;

        ImageDataSection = new ImageDataSectionReader(BinaryReader, this).Value;
        }
    }

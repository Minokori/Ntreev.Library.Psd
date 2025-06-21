using Ntreev.Library.Psd.Services;

namespace Ntreev.Library.Psd;


/// <summary>
/// 植入的其他 PSD document.
/// </summary>
internal class EmbeddedLayer : ILinkedLayer
    {
    private readonly PsdUriResolver resolver = PsdService.Resolver;

    public EmbeddedLayer(Guid id, Uri absoluteUri)
        {
        ID = id;
        AbsoluteUri = absoluteUri;

        if (File.Exists(AbsoluteUri.LocalPath) == true)
            {
            var header = FileHeaderSection.FromFile(this.AbsoluteUri.LocalPath);
            Width = header.Width;
            Height = header.Height;
            }
        }

    public PsdDocument Document
        {
        get
            {
            field ??= resolver.GetDocument(this.AbsoluteUri);
            return field;
            }
        }

    public Uri AbsoluteUri { get; }

    public bool HasDocument => File.Exists(this.AbsoluteUri.LocalPath);

    public Guid ID { get; }

    public string Name => this.AbsoluteUri.LocalPath;

    public int Width { get; }

    public int Height { get; }
    }

using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Services;

namespace Ntreev.Library.Psd;

/// <summary>
/// 嵌入的其他 PSD document.
/// </summary>
internal class EmbeddedLayer(JObject info) : ILinkedLayer
    {
    private readonly IDocumentManager resolver = PsdService.Resolver;
    public FileHeaderSection FileHeader
        {
        get
            {
            field ??= FileHeaderSection.FromFile(AbsoluteUri.LocalPath);
            return field;
            }
        }
    public JObject Properties { get; } = info;


    public PsdDocument Document => resolver.GetDocument(AbsoluteUri);

    //from Properties
    public Uri AbsoluteUri
        {
        get
            {
                {
                var paths = Properties
                    .SelectTokens("$..TEXT")
                    .OfType<JValue>()
                    .Select(x => x.Value<string>())
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(x => Path.GetFullPath(x!))
                    .Where(File.Exists);
                return new(paths.First());
                }
            }
        }

    public bool HasDocument => true;

    public Guid ID => Properties.ToValue<Guid>("UniqueId");

    public string Name => AbsoluteUri.LocalPath;

    public int Width => FileHeader.Width;

    public int Height => FileHeader.Height;
    }

using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Interfaces;
using Ntreev.Library.Psd.Sections;
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
                //查找所有带PAth的属性
                //DescriptorOfLinkedFile
                //BUG
                //var paths = Properties
                //    .SelectTokens("DescriptorOfLinkedFile['Nm','fullPath','relPath']")
                //    .Select(x => x.Value<string>())
                //    .Where(x => !string.IsNullOrEmpty(x));

                //paths = paths
                //    .Select(x => Path.GetFullPath(x!))
                //    .Where(File.Exists);

                //return new(paths.First());

                return Properties.ToValue<Uri>("DescriptorOfLinkedFile.fullPath");
                }
            }
        }

    public bool HasDocument => true;

    public Guid ID => Properties.ToValue<Guid>("UniqueId");

    public string Name => AbsoluteUri.LocalPath;

    public int Width => FileHeader.Width;

    public int Height => FileHeader.Height;
    }

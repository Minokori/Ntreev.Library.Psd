using Ntreev.Library.Psd.Services;

namespace Ntreev.Library.Psd;

public partial class PsdDocument
    {
    public PsdDocument() { }
    #region static classmethod for init
    public static PsdDocument Create(string filename) => Create(filename, PsdService.Resolver);

    public static PsdDocument Create(string filename, PsdUriResolver resolver)
        {
        PsdDocument document = new();
        FileInfo fileInfo = new(filename);
        FileStream stream = new(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
        document.Read(stream, resolver, new Uri(fileInfo.DirectoryName!));
        return document;
        }

    public static PsdDocument Create(Stream stream) => Create(stream, null);

    public static PsdDocument Create(Stream stream, PsdUriResolver resolver)
        {
        PsdDocument document = new();
        document.Read(stream, resolver, new Uri(Directory.GetCurrentDirectory()));
        return document;
        }
    #endregion
    }


namespace Ntreev.Library.Psd;

public partial class PsdDocument
    {
    protected PsdDocument() { }
    #region static classmethod for init

    public static PsdDocument Create(string filename)
        {
        FileInfo fileInfo = new(filename);
        FileStream stream = new(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Create(stream, new Uri(fileInfo.DirectoryName!));
        }

    public static PsdDocument Create(Stream stream, Uri? fileUri = null)
        {
        PsdReader reader = new(stream, fileUri);
        PsdDocument document = new() { BinaryReader = reader };
        document.InitSections();
        return document;
        }
    #endregion
    }


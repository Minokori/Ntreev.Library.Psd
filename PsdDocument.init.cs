namespace Ntreev.Library.Psd;

public partial class PsdDocument
    {
    //在创建许多实例时, 推荐使用 Manager进行管理, 以避免频繁的资源分配和释放。
    // 但如果只创建少量实例, 可以直接使用构造函数。
    public PsdDocument(Stream stream, Uri? fileUri = null)
        {
        BinaryReader = new PsdBinaryReader(stream, fileUri ?? new Uri(Directory.GetCurrentDirectory()));
        InitSections();
        }

    public PsdDocument(string filename)
        : this(File.OpenRead(filename), new Uri(new FileInfo(filename).FullName))
        {
        }

    }


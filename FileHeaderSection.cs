using Ntreev.Library.Psd.Readers;
namespace Ntreev.Library.Psd;

public struct FileHeaderSection
    {
    public int Depth { get; set; }

    public int NumberOfChannels { get; set; }

    public ColorMode ColorMode { get; set; }

    public int Height { get; set; }

    public int Width { get; set; }

    public static FileHeaderSection FromFile(string filename)
        {
        using var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var reader = new PsdReader(stream) { Uri = new(Path.GetDirectoryName(filename)) };
        reader.ReadDocumentHeader();
        return FileHeaderSectionReader.Read(reader);
        }
    }

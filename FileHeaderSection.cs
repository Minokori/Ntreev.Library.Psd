using Ntreev.Library.Psd.Readers;

namespace Ntreev.Library.Psd;

public struct FileHeaderSection
    {
    public string Signature { get; internal set; }
    public int Version { get; internal set; }
    public int Reserved { get; init; }

    public int NumberOfChannels { get; set; }

    public int Height { get; set; }

    public int Width { get; set; }

    public int Depth { get; set; }
    public ColorMode ColorMode { get; set; }

    public static FileHeaderSection FromFile(string filename)
        {
        using var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var reader = new PsdBinaryReader(stream) { Uri = new(Path.GetDirectoryName(filename)) };
        return FileHeaderSectionReader.Read(reader);
        }
    }

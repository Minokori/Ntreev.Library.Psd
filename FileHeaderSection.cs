using Ntreev.Library.Psd.Readers;

namespace Ntreev.Library.Psd;

public class FileHeaderSection : Properties
    {
    public string Signature => this[nameof(Signature)]!.ToObject<string>()!;
    public int Version => this[nameof(Version)]!.ToObject<int>();
    public int Reserved => this[nameof(Reserved)]!.ToObject<int>();

    public int NumberOfChannels => this[nameof(NumberOfChannels)]!.ToObject<int>();

    public int Height => this[nameof(Height)]!.ToObject<int>();

    public int Width => this[nameof(Width)]!.ToObject<int>();

    public int Depth => this[nameof(Depth)]!.ToObject<int>();
    public ColorMode ColorMode => Enum.Parse<ColorMode>(this[nameof(ColorMode)]!.ToObject<string>()!);

    public static FileHeaderSection FromFile(string filename)
        {
        using var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var reader = new PsdBinaryReader(stream) { Uri = new(System.IO.Path.GetDirectoryName(filename)!) };
        return FileHeaderSectionReader.Read(reader);
        }
    }

using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Readers;

namespace Ntreev.Library.Psd.Sections;

/// <summary>
/// 文件的第一部分,包含文件的基本信息
/// </summary>
/// <remarks>
/// 原则上, 不包括图层之间相互引用的纯结构化数据直接用 <see cref="JObject"/> 表示, 考虑到链接 Psd 文件的懒加载, 为文件头部分提供了一个专门的类 <see cref="FileHeaderSection"/>。
/// </remarks>
public class FileHeaderSection : JObject
    {
    public string Signature => this.ToValue<string>(nameof(Signature))!;

    public int Version => this[nameof(Version)]!.ToObject<int>();

    public int Reserved => this[nameof(Reserved)]!.ToObject<int>();

    public int NumberOfChannels => this[nameof(NumberOfChannels)]!.ToObject<int>();

    public int Height => this[nameof(Height)]!.ToObject<int>();

    public int Width => this[nameof(Width)]!.ToObject<int>();

    public int Depth => this[nameof(Depth)]!.ToObject<int>();

    public ColorMode ColorMode =>
        Enum.Parse<ColorMode>(this[nameof(ColorMode)]!.ToObject<string>()!);

    public static FileHeaderSection FromFile(string filename)
        {
        using var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var reader = new PsdBinaryReader(stream, new(System.IO.Path.GetFullPath(filename)));
        return new FileHeaderSectionReader(reader).Value;
        }
    }

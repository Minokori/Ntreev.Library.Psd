using System.Text;
using Ntreev.Library.Psd.Exceptions;
using Ntreev.Library.Psd.Services;

namespace Ntreev.Library.Psd;

internal partial class PsdReader(Stream stream) : BinaryReader(stream)
    {

    public static PsdUriResolver Resolver => PsdService.Resolver;
    public int Version
        {
        get => field;
        set
            {
            if (value is not 1 and not 2)
                throw new InvalidFormatException(
                    "Invalid PSD version. Only version 1 and 2 are supported."
                );
            field = value;
            }
        } = 1;

    public long Position
        {
        get => BaseStream.Position;
        set => BaseStream.Position = value;
        }

    public long Length => BaseStream.Length;

    public Stream Stream => BaseStream;

    public Uri? Uri { get; init; }

    #region 以 "ReadAs" 开头的方法, 功能类似于 BinaryReader 的 "Read" 开头方法, 但会返回特定格式的字符串或数据
    /// <summary>
    /// 从流中读取一个 Pascal 字符串，字符串长度由第一个字节指定，后续字节为字符串内容。<para/>
    /// 该 Pascal 字符串的长度必须是 <paramref name="alignmentSize"/> 的倍数。
    /// </summary>
    /// <param name="alignmentSize">对齐长度</param>
    /// <returns>Pascal 字符串</returns>
    public string ReadAsPascalString(int alignmentSize)
        {
        var count = ReadByte();
        if (count == 0)
            {
            BaseStream.Position += alignmentSize - 1; // 至少读取 length 个字节, count已经读取了一个字节, 因此再读取 length -1 个
            return string.Empty;
            }

        var bytes = ReadBytes(count);
        var text = Encoding.UTF8.GetString(bytes);
        for (var totalLength = count + 1; (totalLength % alignmentSize) != 0; totalLength++)
            {
            BaseStream.Position += 1L; // 跳过填充字节
            }

        return text;
        }

    /// <summary>
    /// 读取指定长度的字节，并以 ASCII 码的形式解析
    /// </summary>
    /// <param name="length">要读取的字节/字符串长度</param>
    /// <returns>长度为 <paramref name="length"/> 的字符串</returns>
    public string ReadAsAscii(int length)
        {
        var bytes = ReadBytes(length);
        return Encoding.ASCII.GetString(bytes);
        }

    /// <summary>
    /// 读取 4 字节的 ASCII 字符串，通常用于读取 PSD 文件类型标识符
    /// </summary>
    /// <returns></returns>
    public string ReadAsType() => ReadAsAscii(4);

    public string ReadAsKey()
        {
        var length = ReadInt32();
        length = (length > 0) ? length : 4;
        return ReadAsAscii(length);
        }

    #endregion


    public long ReadLength() => Version == 1 ? ReadInt32() : ReadInt64();

    public void Skip(char c)
        {
        var ch = ReadChar();
        if (ch != c)
            throw new NotSupportedException();
        }

    public void Skip(char c, int count)
        {
        for (var i = 0; i < count; i++)
            {
            Skip(c);
            }
        }

    public ColorMode ReadColorMode() => (ColorMode)ReadInt16();

    public BlendMode ReadBlendMode() => PsdUtility.ToBlendMode(ReadAsAscii(4));

    public LayerFlags ReadLayerFlags() => (LayerFlags)ReadByte();

    public ChannelType ReadChannelType() => (ChannelType)ReadInt16();

    public CompressionType ReadCompressionType() => (CompressionType)ReadInt16();

    public void ReadDocumentHeader()
        {
        if (!ValidateDocumentSignature())
            throw new InvalidFormatException("Invalid PSD file signature. Expected '8BPS'.");
        Version = ReadInt16();
        Skip(6);
        }

    /// 验证
    public void ValidateSignature(string signature)
        {
        var s = ReadAsType();
        if (s != signature)
            throw new InvalidFormatException();
        }

    public void ValidateSignature() => ValidateSignature(false);

    public void ValidateSignature(bool check64bit)
        {
        if (VerifySignature(check64bit) == false)
            throw new InvalidFormatException();
        }

    private bool ValidateDocumentSignature()
        {
        var signature = ReadAsType();
        return signature == "8BPS";
        }

    public void ValidateInt16(short value, string name)
        {
        var x = ReadInt16();
        if (x != value)
            throw new InvalidFormatException($"The value of {name} is not {value}, but {x}.");
        }

    public void ValidateInt32(int value, string name)
        {
        var x = ReadInt32();
        if (x != value)
            throw new InvalidFormatException($"The value of {name} is not {value}, but {x}.");
        }
    /// <summary>
    /// 读取类型并验证其是否与预期的类型匹配。<para/>
    /// 该操作会导致流位置的变化，因此在调用此方法后，流位置将指向类型字符串之后的位置。
    /// </summary>
    /// <param name="value"></param>
    /// <param name="name"></param>
    /// <exception cref="InvalidFormatException"></exception>
    public void ValidateType(string value, string name = "")
        {
        var type = ReadAsType();
        if (type != value)
            {
            throw new InvalidFormatException(
                $"Invalid type for {name}. Expected '{value}', but got '{type}'."
            );
            }
        }
    }





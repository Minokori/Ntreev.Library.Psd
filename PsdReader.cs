using System.Text;
using Ntreev.Library.Psd.Exceptions;

namespace Ntreev.Library.Psd;

/// <summary>
/// 读取 PSD 文件的二进制数据流的读取器。<para/>
/// </summary>
/// <param name="stream">PSD 文件的数据流</param>
/// <param name="uri">PSD 文件的绝对 Uri. 若 <paramref name="stream"/> 没有对应的文件, 默认 Uri 为当前工作路径</param>
internal partial class PsdBinaryReader(Stream stream, Uri? uri = null) : BinaryReader(stream)
    {
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

    /// <summary>
    /// 根据 PSD 文件头内的 Version 读取一个 <see cref="int"/> 或 <see cref="long"/>, 通常作为数据结构流的长度.
    /// </summary>
    /// <returns>数据结构的字节长度</returns>
    /// <remarks>
    /// 一般而言, PSD 文件的 Version <b>始终</b>为 1.<para/>
    /// PSB 文件的 Version 为 2, 但本程序集不支持 PSB 文件的读取.<para/>
    /// </remarks>
    public long ReadAsStreamLength() => Version == 1 ? ReadInt32() : ReadInt64();

    /// <summary>
    /// 读取一个 <see cref="short"/> 类型的值, 并作为 <see cref="ColorMode"/> 枚举类型返回。<para/>
    /// </summary>
    /// <returns><see cref="ColorMode"/></returns>
    public ColorMode ReadAsColorMode() => (ColorMode)ReadInt16();

    /// <summary>
    /// 读取为长度为4个字节的 ASCII 字符串, 并作为 <see cref="BlendMode"/> 枚举类型返回。<para/>
    /// </summary>
    /// <returns><see cref="BlendMode"/></returns>
    public BlendMode ReadAsBlendMode() => PsdUtility.ToBlendMode(ReadAsAscii(4));

    /// <summary>
    /// 读取一个字节, 并作为 <see cref="LayerFlags"/> 枚举类型返回。<para/>
    /// </summary>
    /// <returns><see cref="LayerFlags"/></returns>
    public LayerFlags ReadAsLayerFlags() => (LayerFlags)ReadByte();

    /// <summary>
    /// 读取一个 <see cref="short"/> 类型的值, 并作为 <see cref="ChannelType"/> 枚举类型返回。<para/>
    /// </summary>
    /// <returns> <see cref="ChannelType"/></returns>
    public ChannelType ReadAsChannelType() => (ChannelType)ReadInt16();

    /// <summary>
    /// 读取一个 <see cref="short"/> 类型的值, 并作为 <see cref="CompressionType"/> 枚举类型返回。<para/>
    /// </summary>
    /// <returns><see cref="CompressionType"/></returns>
    public CompressionType ReadCompressionType() => (CompressionType)ReadInt16();
    #endregion

    #region Skip 方法. 功能类似于 Read 方法, 但不关心读取的内容, 只关心跳过多少字节
    /// <summary>
    /// 跳过指定数量的指定字节
    /// </summary>
    /// <param name="c">要跳过的字节 (ascii 码形式)</param>
    /// <param name="repeat">跳过多少次, 默认为 1</param>
    /// <exception cref="NotSupportedException"></exception>
    public void Skip(char c, int repeat = 1)
        {
        for (var i = 0; i < repeat; i++)
            {
            var readChar = ReadChar();
            if (readChar != c)
                throw new NotSupportedException(
                    $"expect skip {c}, but met {readChar} at {i + 1}th position"
                );
            }
        }
    #endregion

    #region Verify 方法. 用于验证读取的数据是否符合预期

    /// <summary>
    /// 验证读取的签名是否与预期的签名(之一)匹配。<para/>
    /// 从字节流中读取 4 字节的 ASCII 字符串，并与提供的签名进行比较。<para/>
    /// </summary>
    /// <param name="signature">签名/类型, 每一个都是长度为 4 的 ASCII 字符串</param>
    /// <exception cref="InvalidFormatException"></exception>
    /// <remarks>
    /// <b>注意: 该方法将移动字节流的 Position</b>
    /// </remarks>
    public void VerifySignatureIs(params string[] signature)
        {
        var readSignature = ReadAsType();
        if (signature.Contains(readSignature))
            return;
        throw new InvalidFormatException(
            $"Expected signature/type is one of {string.Join(", ", signature)}, but got '{readSignature}'."
        );
        }

    /// <summary>
    /// 验证读取的 <see cref="int"/> 或 <see cref="short"/> 值是否与预期的值匹配。<para/>
    /// 从字节流中读取一个 <see cref="int"/> 或 <see cref="short"/> 值，并与提供的值进行比较。<para/>
    /// </summary>
    /// <typeparam name="T"> <see cref="int"/> 或 <see cref="short"/></typeparam>
    /// <param name="value">要验证的值</param>
    /// <exception cref="InvalidFormatException"></exception>
    /// <remarks>
    /// <b>注意: 该方法将移动字节流的 Position</b>
    /// </remarks>
    public void VerifyIntIs<T>(T value) where T : struct
        {
        switch (value)
            {
            case int intValue:
                { if (intValue == ReadInt32()) return; break; }
            case short shortValue:
                { if (shortValue == ReadInt16()) return; break; }
            }

        throw new InvalidFormatException($"expect {typeof(T)} value {value}");
        }
    #endregion
    }

using Ntreev.Library.Psd.Exceptions;

namespace Ntreev.Library.Psd;

internal abstract partial class ValueReader<T>
    {
    /// <summary>
    /// 读取指定数据类型的类, 该类从 <see cref="PsdReader"/> 中读取数据。
    /// </summary>
    /// <param name="reader">全局 BinaryReader, 每个 <see cref="PsdDocument"/> 具有一个</param>
    /// <param name="hasLength">要读取的数据结构是否包括一个前导块以指示该数据块的字节数</param>
    /// <param name="userData"></param>
    protected ValueReader(PsdReader reader, bool hasLength, object? userData)
        {
        GlobalReader = reader;
        if (hasLength == true)
            {
            StreamLength = InitStreamLength();
            }

        ReaderVersion = reader.Version;
        StartPosition = reader.Position;
        UserData = userData;

        if (hasLength == false)
            {
            Value = InitValue();
            StreamLength = reader.Position - StartPosition;
            }

        reader.Position = StartPosition + StreamLength;
        }


    /// <summary>
    ///  读取指定数据类型的类, 该类从 <see cref="PsdReader"/> 中读取数据。
    /// </summary>
    /// <param name="reader">全局 BinaryReader, 每个 <see cref="PsdDocument"/> 具有一个</param>
    /// <param name="length">数据的字节数</param>
    /// <param name="userData"></param>
    /// <exception cref="InvalidFormatException"></exception>
    protected ValueReader(PsdReader reader, long length, object? userData)
        {
        if (length < 0)
            throw new InvalidFormatException();
        GlobalReader = reader;
        StreamLength = length;
        ReaderVersion = reader.Version;
        StartPosition = reader.Position;
        UserData = userData;

        if (StreamLength == 0)
            {
            Value = InitValue();
            StreamLength = reader.Position - StartPosition;
            }

        reader.Position = StartPosition + StreamLength;
        }
    }

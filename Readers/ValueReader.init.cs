using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Exceptions;

namespace Ntreev.Library.Psd;

internal abstract partial class ValueReader<T>
    {
    /// <summary>
    /// 读取指定数据类型的类, 该类从 <see cref="PsdBinaryReader"/> 中读取数据。
    /// </summary>
    /// <param name="reader">全局 BinaryReader, 每个 <see cref="PsdDocument"/> 具有一个</param>
    /// <param name="hasLength">要读取的数据结构是否包括一个前导块以指示该数据块的字节数</param>
    /// <param name="userData"></param>
    protected ValueReader(PsdBinaryReader reader, bool hasLength, object? userData)
        {
        GlobalReader = reader;

        // 数据结构长度已知
        if (hasLength == true)
            {
            StreamLength = InitStreamLength();
            }


        StartPosition = reader.Position;
        UserData = userData;

        //数据结构长度未知, 将实际读取数据, 根据读取数据后的流位置计算长度
        if (hasLength == false)
            {
            Value = ReadValue();
            StreamLength = reader.Position - StartPosition;
            }


        // 将 GlobalReader 的位置指针移动到数据块的结束位置, 以便下一个ValueReader 可以继续读取
        reader.Position = EndPosition;

        if (typeof(T) != typeof(JObject) && (typeof(T) != typeof(JArray)))
            Debug.WriteLine($"ValueReader<{typeof(T).Name}>: StartPosition={StartPosition}, StreamLength={StreamLength}");
        }


    /// <summary>
    ///  读取指定数据类型的类, 该类从 <see cref="PsdBinaryReader"/> 中读取数据。
    /// </summary>
    /// <param name="reader">全局 BinaryReader, 每个 <see cref="PsdDocument"/> 具有一个</param>
    /// <param name="length">数据的字节数</param>
    /// <param name="userData"></param>
    /// <exception cref="InvalidFormatException"></exception>
    protected ValueReader(PsdBinaryReader reader, long length, object? userData)
        {
        if (length < 0)
            throw new InvalidFormatException();


        GlobalReader = reader;
        StreamLength = length;
        StartPosition = reader.Position;
        UserData = userData;

        // 确保数据长度为0 的 Value 被初始化不为 null
        if (StreamLength == 0)
            {
            Value = ReadValue();
            }

        reader.Position = EndPosition;
        if (typeof(T) != typeof(JObject) && (typeof(T) != typeof(JArray)))

            Debug.WriteLine($"ValueReader<{typeof(T).Name}>: StartPosition={StartPosition}, StreamLength={StreamLength}");

        }
    }

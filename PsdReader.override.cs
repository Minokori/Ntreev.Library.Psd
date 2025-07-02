using System.Buffers.Binary;
using System.Text;

namespace Ntreev.Library.Psd;
internal partial class PsdBinaryReader
    {

    #region 由于 PSD 存储采用大端, 所以需要重载读取方法 (BinaryReader使用小端读取)
    public override double ReadDouble()
        {
        var bytes = ReadBytes(8); //double 占用 8 字节
        return BinaryPrimitives.ReadDoubleBigEndian(bytes);
        }

    public override short ReadInt16()
        {
        var bytes = ReadBytes(2); //short 占用 2 字节
        return BinaryPrimitives.ReadInt16BigEndian(bytes);
        }

    public override int ReadInt32()
        {
        var bytes = ReadBytes(4); //int 占用 4 字节
        return BinaryPrimitives.ReadInt32BigEndian(bytes);
        }

    public override long ReadInt64()
        {
        var bytes = ReadBytes(8); //long 占用 8 字节
        return BinaryPrimitives.ReadInt64BigEndian(bytes);
        }

    public override ushort ReadUInt16()
        {
        var bytes = ReadBytes(2); //ushort 占用 2 字节
        return BinaryPrimitives.ReadUInt16BigEndian(bytes);
        }

    public override uint ReadUInt32()
        {
        var bytes = ReadBytes(4); //uint 占用 4 字节
        return BinaryPrimitives.ReadUInt32BigEndian(bytes);
        }

    public override ulong ReadUInt64()
        {
        var bytes = ReadBytes(8); //ulong 占用 8 字节
        return BinaryPrimitives.ReadUInt64BigEndian(bytes);
        }

    public override string ReadString()
        {
        var charNumber = ReadInt32();
        if (charNumber == 0)
            return string.Empty;

        var bytes = ReadBytes(charNumber * 2);
        for (var i = 0; i < charNumber; i++)
            {
            var index = i * 2;
            (bytes[index + 1], bytes[index]) = (bytes[index], bytes[index + 1]);
            }

        if (bytes[^1] == 0 && bytes[^2] == 0)
            {
            charNumber--;
            }

        var name = Encoding.Unicode.GetString(bytes, 0, charNumber * 2);
        return name!;
        }

    public override char ReadChar() => (char)ReadByte();
    #endregion


    #region 其他读取基本数据类型的方法, 不存在于 BinaryReader 中
    /// <summary>
    /// 读取指定数量的 double,并返回一个 double 数组。
    /// </summary>
    /// <param name="count">读取 double 数据的个数</param>
    /// <returns>长度为 <paramref name="count"/> 的 double 数组</returns>
    public double[] ReadDoubles(int count)
        {
        var values = new double[count];
        for (var i = 0; i < count; i++)
            {
            values[i] = ReadDouble();
            }

        return values;
        }
    #endregion
    }

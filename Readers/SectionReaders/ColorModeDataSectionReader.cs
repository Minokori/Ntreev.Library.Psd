namespace Ntreev.Library.Psd.Readers;

/// <summary>
/// 颜色模式数据部分的读取器<para/>
/// 只有索引颜色和双色调（请参阅文件头部分中的模式字段）具有颜色模式数据。对于所有其他模式，此部分只是 4 字节长度的字段，该字段设置为零
/// </summary>
/// <param name="reader"></param>
internal class ColorModeDataSectionReader(PsdBinaryReader reader) : LazyValueReader<byte[]>(reader, null)
    {
    protected override long InitStreamLength() => GlobalReader.ReadInt32();

    protected override byte[] ReadValue() =>
        StreamLength > 0 ? GlobalReader.ReadBytes((int)StreamLength) : [];
    }

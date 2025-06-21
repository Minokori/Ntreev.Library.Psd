namespace Ntreev.Library.Psd;

/// <summary>
/// 读取指定数据类型的类, 该类从 <see cref="PsdReader"/> 中读取数据。<para/>
/// 在实例化后, 仅需要访问其 <see cref="Value"/> 属性即可获取读取的数据。<para/>"/>
/// </summary>
/// <typeparam name="T">读取的数据类型</typeparam>
internal abstract partial class ValueReader<T>
    {
    /// <summary>
    /// 全局 BinaryReader, 每个 <see cref="PsdDocument"/> 具有一个
    /// </summary>
    protected PsdReader GlobalReader { get; init; }

    /// <summary>
    /// 读取数据使用的协议版本, 1 为 32位版本, 2 为 64 位版本
    /// </summary>
    protected int ReaderVersion { get; init; }
    protected object? UserData { get; init; } = null;

    /// <summary>
    /// 数据是否读取过.
    /// </summary>
    private bool HasRead { get; set; }


    /// <summary>
    /// 从 文件流中读取值, 赋值给 <see cref="Value"/>, 将 <see cref="HasRead"/> 设置为 true
    /// </summary>
    private T? InitValue()
        {
        //更新字节流的位置指针，从指定位置开始读取
        GlobalReader.Position = StartPosition;
        GlobalReader.Version = ReaderVersion;

        // 从 reader 提供的字节流中读取值到 value 中
        var value = ReadValue();

        // 更新字节流指针，便于继续读取
        if (StreamLength > 0)
            GlobalReader.Position = StartPosition + StreamLength;
        // 设置为已读取状态
        HasRead = true;
        return value;
        }

    protected virtual long InitStreamLength() => GlobalReader.ReadLength();


    /// <summary>
    /// 使用自己引用的 全局 GlobalReader 对象从字节流中读取值. <para/>
    /// 应重写该方法以实现具体的数据读取逻辑。
    /// </summary>
    protected abstract T ReadValue();
    }

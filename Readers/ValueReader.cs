namespace Ntreev.Library.Psd;

/// <summary>
/// 读取指定数据类型的类, 该类从 <see cref="PsdBinaryReader"/> 中读取数据。<para/>
/// 在实例化后, 仅需要访问其 <see cref="Value"/> 属性即可获取读取的数据。<para/>
/// </summary>
/// <typeparam name="T">读取的数据类型</typeparam>
/// <remarks>
/// 子类需要实现 <see cref="ReadValue"/> 方法来定义如何从 <see cref="PsdBinaryReader"/> 中读取数据。<para/>
/// 部分子类需要 <see cref="InitStreamLength"/> 方法。<para/>
/// </remarks>
internal abstract partial class ValueReader<T>
    {
    /// <summary>
    /// 全局 BinaryReader, 每个 <see cref="PsdDocument"/> 具有一个
    /// </summary>
    protected PsdBinaryReader GlobalReader { get; init; }

    protected object? UserData { get; init; } = null;

    /// <summary>
    /// 数据是否读取过.
    /// </summary>
    private bool HasRead { get; set; }


    /// <summary>
    /// 初始化流长度, 该方法默认根据 <see cref="ReaderVersion"/> 版本读取值.<para/>
    /// <see cref="ReaderVersion"/> 为 1 时, 读取 int32, 为 2 时, 读取 int64.<para/>
    /// 可能需要重写该方法以实现特定的流长度读取逻辑。
    /// </summary>
    /// <returns></returns>
    protected virtual long InitStreamLength() => GlobalReader.ReadAsStreamLength();


    /// <summary>
    /// 使用自己引用的 全局 GlobalReader 对象从字节流中读取值并解析. <para/>
    /// 应重写该方法以实现具体的数据读取逻辑。
    /// </summary>
    /// <remarks>
    /// <b>注意: 该方法将移动 <see cref="GlobalReader"/> 的 Position</b><para/>
    /// <b>注意: 不能返回 null</b>
    /// </remarks>
    protected abstract T ReadValue();
    }

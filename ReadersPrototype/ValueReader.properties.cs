namespace Ntreev.Library.Psd;

internal partial class ValueReader<T>
    {


    /// <summary>
    /// 值. (懒加载)
    /// </summary>
    /// <remarks>
    /// 访问该属性会检查缓存, 首次访问时会调用 <see cref="ReadValue"/> 方法从流中读取值.<para/>"
    /// 后续访问将直接返回缓存的值.<para/>
    /// </remarks>
    public T Value
        {
        get
            {
            if (HasRead == false && StreamLength > 0) // 没有数据但应该有数据时, 读取数据
                {
                // 缓存
                var globalPosition = GlobalReader.Position;

                // 更新字节流的位置指针，从数据结构实际存在的位置开始读取
                GlobalReader.Position = StartPosition;

                // 读取值
                Value = ReadValue();

                // 恢复全局 BinaryReader 的位置和版本
                GlobalReader.Position = globalPosition;
                }

            return field;
            }
        private set
            {
            field = value;
            HasRead = true;
            }
        }

    /// <summary>
    /// 值占用的字节长度
    /// </summary>
    public long StreamLength { get; set; }

    /// <summary>
    /// 值在整个文档二进制流的开始位置
    /// </summary>
    public long StartPosition { get; protected set; }

    /// <summary>
    /// 值在整个文档二进制流的结束位置
    /// </summary>
    public long EndPosition => StartPosition + StreamLength;

    }


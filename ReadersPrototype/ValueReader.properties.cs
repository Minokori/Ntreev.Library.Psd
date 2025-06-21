namespace Ntreev.Library.Psd;

internal partial class ValueReader<T>
    {


    /// <summary>
    /// 值. (懒加载)
    /// </summary>
    public T? Value
        {
        get
            {
            if (HasRead == false && StreamLength > 0) // 没有数据但应该有数据时, 读取数据
                {
                var position = GlobalReader.Position;
                var version = GlobalReader.Version;
                field = InitValue();
                GlobalReader.Position = position;
                GlobalReader.Version = version;
                }
            else if (HasRead == false && StreamLength <= 0) // 没有数据且不应该有数据时, 返回默认值
                {
                field = default!;
                HasRead = true;
                }

            return field;
            }
        private set;
        }

    /// <summary>
    /// 值占用的字节长度
    /// </summary>
    public long StreamLength { get; init; }

    /// <summary>
    /// 值在整个文档二进制流的开始位置
    /// </summary>
    public long StartPosition { get; init; }

    /// <summary>
    /// 值在整个文档二进制流的结束位置
    /// </summary>
    public long EndPosition => StartPosition + StreamLength;

    }


namespace Ntreev.Library.Psd.Interfaces;

/// <summary>
/// 图片对象的颜色通道
/// </summary>
public interface IChannel
    {
    /// <summary>
    /// <code>Data.Length =Depth * Width * Height</code>
    /// <code>Data[ (x*Width+y)Depth ..(x*Width+y+1)Depth ] = 通道 (x,y) 处 的像素值</code>
    /// </summary>
    /// <remarks>
    /// Data 的数据结构为: <para/>
    /// 1 个像素点的数据长度为 Depth, 从左到右, 从上到下排列每个像素点的数据<para/>
    /// <i>实际上由于目前仅支持 Depth = 8 的数据, 恒有 <code>Depth == 1</code>
    /// 也即
    /// <code>Data.Length = Width * Height</code>
    /// <code>Data[ x*Width+y ] = 通道 (x,y) 处 的像素值</code>
    /// </i>
    /// </remarks>
    byte[] Data { get; }

    /// <summary>
    /// 通道颜色类型
    /// </summary>
    ChannelType Type { get; }

    // TODO try to make  data into a matrix
    int Width { get; init; }
    int Height { get; init; }
    int Depth { get; init; }
    /// <summary>
    /// shape = h,w
    /// </summary>
    }

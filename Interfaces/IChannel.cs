namespace Ntreev.Library.Psd;

/// <summary>
/// 通道接口
/// </summary>
public interface IChannel
    {
    /// <summary>
    /// 通道数据
    /// </summary>
    byte[] Data { get; }

    /// <summary>
    /// 通道类型
    /// </summary>
    ChannelType Type { get; }
    }

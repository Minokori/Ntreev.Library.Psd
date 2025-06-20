namespace Ntreev.Library.Psd;

/// <summary>
/// 图片源接口
/// </summary>
public interface IImageSource
    {
    int Width { get; }

    int Height { get; }

    int Depth { get; }

    IChannel[] Channels { get; }

    float Opacity { get; }

    bool HasImage { get; }

    bool HasMask { get; }
    }


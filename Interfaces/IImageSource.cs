namespace Ntreev.Library.Psd.Interfaces;

/// <summary>
/// 图片源接口, 包括 <see cref="IChannel"/> 和其他相关属性"/>
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


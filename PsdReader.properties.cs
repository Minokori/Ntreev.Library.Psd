using Ntreev.Library.Psd.Exceptions;
using Ntreev.Library.Psd.Sections;
using Ntreev.Library.Psd.Services;
namespace Ntreev.Library.Psd;

internal partial class PsdBinaryReader
    {
    /// <summary>
    /// 持有的对单例 <see cref="IDocumentManager"/> 的引用。
    /// </summary>
    public static IDocumentManager Resolver => PsdService.Resolver;

    /// <summary>
    /// PSD 文件的版本. 存在于 <see cref="FileHeaderSection"/> 中
    /// </summary>
    /// <remarks>
    /// 始终等于 1。如果与此值不匹配，请不要尝试读取文件。PSB 为 2。
    /// </remarks>
    public int Version
        {
        get => field;
        set
            {
            if (value is not 1 and not 2)
                throw new InvalidFormatException(
                    "Invalid PSD version. Only version 1 and 2 are supported."
                );
            field = value;
            }
        } = 1;

    /// <summary>
    /// 字节流当前的位置。<para/>
    /// </summary>
    public long Position
        {
        get => BaseStream.Position;
        set => BaseStream.Position = value;
        }

    /// <summary>
    /// 字节流的长度
    /// </summary>
    public long Length => BaseStream.Length;

    /// <summary>
    /// 字节流
    /// </summary>
    public Stream Stream => BaseStream;


    /// <summary>
    /// PSD 文件的 URI。<para/>
    /// </summary>
    public Uri? Uri
        {
        get;
        } = uri ?? new Uri(Directory.GetCurrentDirectory());
    }



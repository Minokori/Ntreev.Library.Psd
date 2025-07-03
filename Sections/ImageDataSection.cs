using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Interfaces;

namespace Ntreev.Library.Psd.Sections;
internal class ImageDataSection(int width, int height, int depth, Channel[] channels) : IPsdLayer
    {
    public BlendMode BlendMode => BlendMode.Normal;

    public IPsdLayer[] Childs => [];

    public bool IsClipping => false;

    public ILinkedLayer? LinkedLayer => null;

    public string Name => "Preview Image";

    public IPsdLayer Parent => Document;

    public JObject Resources => [];

    public required PsdDocument Document { get; init; }

    public int Left => 0;

    public int Top => 0;

    public int Right => width;

    public int Bottom => height;

    public int Width => width;

    public int Height => height;

    public int Depth => depth;

    public IChannel[] Channels => channels;

    public float Opacity => 1;

    public bool HasImage => true;

    public bool HasMask => false;

    }

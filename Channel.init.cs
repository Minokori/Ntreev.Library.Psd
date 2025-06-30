using Newtonsoft.Json.Linq;

namespace Ntreev.Library.Psd;

internal partial class Channel
    {
    public Channel(ChannelType type, int width, int height, long size, int depth)
        {
        Type = type;
        Width = width;
        Height = height;
        StreamLength = size;
        Depth = depth;
        }
    }


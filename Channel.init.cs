namespace Ntreev.Library.Psd;

internal partial class Channel
    {
    public Channel()
        {

        }
    public Channel(ChannelType type, int width, int height, long size)
        {
        Type = type;
        Width = width;
        Height = height;
        StreamLength = size;
        }
    }


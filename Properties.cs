using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ntreev.Library.Psd;

public class Properties : JObject
    {
    private readonly List<ILinkedLayer> linkedLayers = [];
    public void AddLayers(List<ILinkedLayer> layers) => linkedLayers.AddRange(layers);

    [JsonIgnore]
    public ILinkedLayer[] LinkedLayers => [.. linkedLayers];
    }

/// <summary>
/// backup for some time use
/// </summary>
public static class JObjectExtensions
    {
    extension(JObject jObject)
        {
        public T? ToValue<T>(params string[] properties)
            {
            foreach (var property in properties)
                {
                var token = jObject.SelectToken(property);
                if (token == null)
                    continue;
                else
                    return token.ToObject<T>()!;
                }

            return default;
            }

        public bool Contains(string property, params string[] properties)
            {
            var jQuery = string.Join(".", [property, .. properties]).TrimEnd('.');
            return jObject.SelectToken(jQuery) != null;
            }

        /// <summary>
        /// 从 一个Layer 的 Records 初始化通道(静态方法)
        /// 没有 MetaInfo
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        internal Channel[] InitChannels(int depth)
            {
            var count = jObject.SelectToken("ChannelCount")!.ToObject<int>();
            var Width = jObject.SelectToken("Right")!.ToObject<int>() - jObject.SelectToken("Left")!.ToObject<int>();
            var Height = jObject.SelectToken("Bottom")!.ToObject<int>() - jObject.SelectToken("Top")!.ToObject<int>();
            if ((Width > 0x3000) || (Height > 0x3000))
                {
                throw new NotSupportedException($"Invalidated size ({Width}, {Height})");
                }


            var channels = new Channel[count];


            for (var i = 0; i < count; i++)
                {

                var type = jObject.SelectToken($"ChannelID[{i}]")!.ToObject<ChannelType>()!;
                var width = type switch
                    {
                        ChannelType.Mask => jObject.SelectToken("Mask.Width") ?? Width,
                        _ => Width,
                        };

                var height = type switch
                    {
                        ChannelType.Mask => jObject.SelectToken("Mask.Height") ?? Height,
                        _ => Height,
                        };
                var c = new Channel()
                    {
                    Type = type,
                    StreamLength = jObject.SelectToken($"ChannelDataLength[{i}]")!.ToObject<long>()!,
                    Height = height.ToObject<int>(),
                    Width = width.ToObject<int>(),
                    Depth = depth,
                    };
                var prop = jObject.SelectToken("Resources")!;
                if (type == ChannelType.Alpha && prop.Contains("iOpa.Opacity"))
                    {
                    var opa = prop.SelectToken("iOpa.Opacity")!.ToObject<byte>();
                    c.Opacity = opa / 255.0f;
                    }

                channels[i] = c;
                }

            return channels;
            }

        }
    }
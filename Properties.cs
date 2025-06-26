using Newtonsoft.Json.Linq;

namespace Ntreev.Library.Psd;

public class Properties : JObject
    {
    private readonly List<ILinkedLayer> linkedLayers = [];

    public bool Contains(string property)
        {
        var a = SelectToken(property);
        return a != null;
        }

    public T ToValue<T>(string property, params string[] properties)
        {
        var jQuery = string.Join(".", [property, .. properties]).TrimEnd('.');

        var token = SelectToken(jQuery);

        return token == null ? throw new KeyNotFoundException($"Property '{jQuery}' not found.") : token.ToObject<T>()!;

        }

    public void AddLayers(List<ILinkedLayer> layers) => linkedLayers.AddRange(layers);

    public ILinkedLayer[] LinkedLayers => [.. linkedLayers];
    }

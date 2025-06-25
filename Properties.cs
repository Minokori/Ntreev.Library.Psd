using System.Collections;
using Newtonsoft.Json.Linq;

namespace Ntreev.Library.Psd;
// propreties 中某个 key-value的 value 可能是 一个 array, 这导致其元素没有key, contains 为了这种情况做了优化
public class Properties(int capacity = 0) : Dictionary<string, object>(capacity)

    {
    public bool Contains(string property)
        {
        var subKeys = property.Split(['.', '[', ']'], StringSplitOptions.RemoveEmptyEntries);

        object value = this;

        foreach (var item in subKeys)
            {
            if ((value is ArrayList) == true)
                {
                var arrayList = value as ArrayList;
                if (int.TryParse(item, out var index) == false)
                    return false;
                if (index >= arrayList.Count)
                    return false;
                value = arrayList[index];
                }
            else if ((value is IDictionary<string, object>) == true)
                {
                var props = value as IDictionary<string, object>;
                if (props.ContainsKey(item) == false)
                    {
                    return false;
                    }

                value = props[item];
                }
            }

        return true;
        }

    private object GetProperty(string property)
        {
        var ss = property.Split(['.', '[', ']'], StringSplitOptions.RemoveEmptyEntries);

        object value = this;

        foreach (var item in ss)
            {
            if ((value is ArrayList) == true)
                {
                var arrayList = value as ArrayList;
                value = arrayList[int.Parse(item)];
                }
            else if ((value is IDictionary<string, object>) == true)
                {
                var props = value as IDictionary<string, object>;
                value = props[item];
                }
            else if ((value is Properties) == true)
                {
                var props = value as Properties;
                value = props[item];
                }
            }

        return value;
        }
    public new object this[string property]
        {
        get => GetProperty(property);
        set => Add(property, value);
        }
    }


public class NewProp : JObject
    {

    }
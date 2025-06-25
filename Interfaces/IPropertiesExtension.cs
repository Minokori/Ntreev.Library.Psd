//Released under the MIT License.
//
//Copyright (c) 2015 Ntreev Soft co., Ltd.
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//documentation files (the "Software"), to deal in the Software without restriction, including without limitation the 
//rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit 
//persons to whom the Software is furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the 
//Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE 
//WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
//OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Ntreev.Library.Psd;
public static class IKVExtension
    {
    /// <summary>
    /// 生成 prop.subprop1.subprop2 的属性名称
    /// </summary>
    /// <param name="property"></param>
    /// <param name="properties"></param>
    /// <returns></returns>
    private static string GeneratePropertyName(string property, params string[] properties)
        {
        if (properties.Length == 0)
            return property;

        var pname = property + "." + string.Join(".", properties);
        return pname;
        }

    public static T ToValue<T>(this Properties props, string property, params string[] properties)
        {
        var jQuery = (property + "." + string.Join(".", properties)).TrimEnd(".").ToString();
        var token = props.SelectToken(jQuery);
        return token == null ? throw new KeyNotFoundException($"Property '{jQuery}' not found.") : token.ToObject<T>();

        //var propertyName = GeneratePropertyName(property, properties);
        //var parts = propertyName.Split('.');
        //object? current = null;
        //var currentProps = props;

        //for (var i = 0; i < parts.Length; i++)
        //    {
        //    var part = parts[i];
        //    var found = currentProps.FirstOrDefault(kv => kv.Key == part);

        //    if (found.Key == null)
        //        throw new KeyNotFoundException($"Property '{propertyName}' not found.");

        //    if (i == parts.Length - 1)
        //        {
        //        current = found.Value;
        //        }
        //    else
        //        {
        //        currentProps = found.Value is Properties nested
        //            ? nested
        //            : found.Value is Properties nestedProps
        //                ? nestedProps
        //                : throw new InvalidOperationException($"Property '{part}' is not a nested property.");
        //        }
        //    }

        //return current is T t
        //    ? t
        //    : current is IConvertible
        //    ? (T)Convert.ChangeType(current, typeof(T))
        //    : current is not null
        //    ? (T)current
        //    : throw new InvalidCastException($"Cannot convert property '{propertyName}' to type '{typeof(T)}'.");
        }

    public static Guid ToGuid(this Properties props, string property, params string[] properties) => new(props.ToString(property, properties));

    public static string ToString(this Properties props, string property, params string[] properties) => ToValue<string>(props, property, properties);

    public static byte ToByte(this Properties props, string property, params string[] properties) => ToValue<byte>(props, property, properties);

    public static int ToInt32(this Properties props, string property, params string[] properties) => ToValue<int>(props, property, properties);

    public static float ToSingle(this Properties props, string property, params string[] properties) => ToValue<float>(props, property, properties);

    public static double ToDouble(this Properties props, string property, params string[] properties) => ToValue<double>(props, property, properties);

    public static bool ToBoolean(this Properties props, string property, params string[] properties) => ToValue<bool>(props, property, properties);

    public static bool TryGetValue<T>(this Properties props, ref T value, string property, params string[] properties)
        {
        var propertyName = GeneratePropertyName(property, properties);
        if (props.Contains(propertyName) == false)
            return false;
        value = props.ToValue<T>(propertyName);
        return true;
        }



    }



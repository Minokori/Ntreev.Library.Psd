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

using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Attributes;
using Ntreev.Library.Psd.Readers.ResourceReader;

namespace Ntreev.Library.Psd.Readers;

/// <summary>
/// 静态类型, 用于通过反射自动创建实例。
/// </summary>
internal static class ReaderCollector
    {
    /// <summary>
    /// 保存 资源ID (长度为4的字符串) - 读取器类型 的映射关系。
    /// </summary>
    private static Dictionary<string, Type> Readers { get; }

    /// <summary>
    /// 静态构造函数
    /// </summary>
    static ReaderCollector()
        {
        var assembly = typeof(ValueReader<JToken>).Assembly;

        var query = assembly
            .GetTypes()
            .Where(item =>
                typeof(ValueReader<JToken>).IsAssignableFrom(item)
                && (item.Attributes & TypeAttributes.Abstract) != TypeAttributes.Abstract
            );

        Readers = new Dictionary<string, Type>(query!.Count());

        foreach (var readerType in query!)
            {
            var attributes = readerType.GetCustomAttributes(typeof(ResourceIDAttribute), true);
            if (attributes.Length == 0)
                continue;
            var resourceID = (ResourceIDAttribute)attributes.First();

            foreach (var id in resourceID.ID)
                {
                Readers.Add(id, readerType);
                }

            //Readers.Add(resourceID.ID, readerType);
            }
        }

    public static ValueReader<JToken> CreateReader(
        string resourceID,
        PsdBinaryReader reader,
        long length
    )
        {
        var readerType = Readers.TryGetValue(resourceID, out var type)
            ? type
            : typeof(EmptyResourceReader);

        var readerInstance = TypeDescriptor.CreateInstance(
            null,
            readerType,
            [typeof(PsdBinaryReader), typeof(long)],
            [reader, length]
        );
        return (ValueReader<JToken>)readerInstance!;
        }

    private static string GetDisplayName(Type type)
        {
        var resourceIds = type.GetCustomAttributes(typeof(ResourceIDAttribute), true);
        var resourceId = resourceIds.First() as ResourceIDAttribute;
        return resourceId?.DisplayName ?? "";
        }

    // TODO 获取的资源名称会影响到jQuery
    public static string GetDisplayName(string resourceID) =>
        Readers.ContainsKey(resourceID) == true ? GetDisplayName(Readers[resourceID]) : resourceID;
    }

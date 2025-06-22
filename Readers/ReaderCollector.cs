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
using Ntreev.Library.Psd.Attributes;
using Ntreev.Library.Psd.ReadersPrototype;

namespace Ntreev.Library.Psd.Readers;

internal static class ReaderCollector
    {
    private static Dictionary<string, Type> Readers { get; }

    static ReaderCollector()
        {
        var assembly = typeof(ResourceReaderBase).Assembly;

        var query = assembly?
            .GetTypes()
            .Where(item =>
                typeof(ResourceReaderBase).IsAssignableFrom(item)
                && (item.Attributes & TypeAttributes.Abstract) != TypeAttributes.Abstract
            );

        Readers = new Dictionary<string, Type>(query!.Count());

        foreach (var readerType in query!)
            {
            var attributes = readerType.GetCustomAttributes(typeof(ResourceIDAttribute), true);

            if (attributes.Length == 0)
                continue;

            var resourceID = (ResourceIDAttribute)attributes.First();
            Readers.Add(resourceID.ID, readerType);
            }
        }

    public static ResourceReaderBase CreateReader(string resourceID, PsdBinaryReader reader, long length)
        {
        var readerType = typeof(EmptyResourceReader);
        if (Readers.ContainsKey(resourceID) == true)
            {
            readerType = Readers[resourceID];
            }

        var readerInstance = TypeDescriptor.CreateInstance(
                null,
                readerType,
                [typeof(PsdBinaryReader), typeof(long)],
                [reader, length]
            );

        return (ResourceReaderBase)readerInstance!;
        }

    public static string GetDisplayName(Type type)
        {
        var attrs = type.GetCustomAttributes(typeof(ResourceIDAttribute), true);

        var attr = attrs.First() as ResourceIDAttribute;
        return attr.DisplayName;
        }

    public static string GetDisplayName(string resourceID) =>
        Readers.ContainsKey(resourceID) == true ? GetDisplayName(Readers[resourceID]) : resourceID;
    }

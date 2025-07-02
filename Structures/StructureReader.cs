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

using Newtonsoft.Json.Linq;

namespace Ntreev.Library.Psd.Structures;

internal static partial class StructureReader
    {

    public static JObject ReadDescriptor(PsdBinaryReader reader)
        {
        JObject obj = [];
        var _ = reader.ReadInt32();// Version, not used

        obj.Add("Name", reader.ReadString());
        obj.Add("ClassID", reader.ReadAsKey());

        var count = reader.ReadInt32();
        for (var i = 0; i < count; i++)
            {
            // key, and osType, both 4 bytes
            var key = reader.ReadAsKey();
            var osType = reader.ReadAsType();
            var prop = Read(osType, reader);
            obj.Add(key.Trim(), prop);
            }

        return obj;


        }

    public static JToken Read(string osType, PsdBinaryReader reader)
        {
        return osType switch
            {
                // 基本数据类型, 返回 JValue
                "doub" => ReadDouble(reader),
                "TEXT" => ReadString(reader),
                "long" => ReadInt32(reader),
                "bool" => ReadBoolean(reader),
                "comp" => ReadInt64(reader),

                // 简单数据类型 (没有嵌套包含其他简单类型), 返回 JObject
                "prop" => ReadProperty(reader),
                "UntF" => ReadUnitFloat(reader),
                "type" => ReadClass(reader),
                "GlbC" => ReadClass(reader),
                "Clss" => ReadClass(reader),
                "enum" => ReadEnumerate(reader),
                "Enmr" => ReadEnumerateReference(reader), //TODO 和文档描述不一致,修改前和上面一行一样
                "alis" => ReadAlias(reader),
                "rele" => ReadOffset(reader),
                "tdta" => ReadEngineData(reader),

                //依赖其他的Structure
                "obj" => ReadReference(reader),
                //会导致递归调用
                "VlLs" => ReadStructureList(reader),
                "Objc" => ReadSubDescriptor(reader),
                "GlbO" => ReadSubDescriptor(reader),

                // 不受支持的

                "ObAr" => ReadObjectArray(reader),
                _ => throw new NotSupportedException(osType),
                };
        }
    }

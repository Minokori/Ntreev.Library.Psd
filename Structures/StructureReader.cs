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


namespace Ntreev.Library.Psd.Structures;

internal static class StructureReader
    {
    public static object Read(string ostype, PsdBinaryReader reader)
        {
        return ostype switch
            {
                // base types
                "doub" => reader.ReadDouble(),
                "TEXT" => reader.ReadString(),
                "long" => reader.ReadInt32(),
                "bool" => reader.ReadBoolean(),
                "comp" => reader.ReadInt64(),
                // dictionary (basetype inside) 不依赖其他的结构
                // 考虑拼接出来
                "prop" => new StructureProperty(reader),
                "UntF" => new StructureUnitFloat(reader),
                "type" => new StructureClass(reader),
                "GlbC" => new StructureClass(reader),
                "Clss" => new StructureClass(reader),
                "enum" => new StructureEnumerate(reader),
                "Enmr" => new StructureEnumerateReference(reader),//TODO 和文档描述不一致,修改前和上面一行一样
                "alis" => new StructureAlias(reader),
                "rele" => new StructureOffset(reader),

                //依赖其他的Structure
                "obj" => new StructureReference(reader),
                //会导致递归调用
                "VlLs" => new StructureList(reader),
                "Objc" => new DescriptorStructure(reader, false),
                "GlbO" => new DescriptorStructure(reader, false),

                // 不受支持的
                "tdta" => new StructureUnknownOSType("Cannot read RawData"),
                "ObAr" => new StructureObjectArray(reader),
                _ => throw new NotSupportedException(ostype),
                };
        }
    }

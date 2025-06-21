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
    public static object Read(string ostype, PsdReader reader)
        {
        return ostype switch
            {
                "obj " => new StructureReference(reader),
                "Objc" => new DescriptorStructure(reader, false),
                "VlLs" => new StructureList(reader),
                "doub" => reader.ReadDouble(),
                "UntF" => new StructureUnitFloat(reader),
                "TEXT" => reader.ReadString(),
                "enum" => new StructureEnumerate(reader),
                "long" => reader.ReadInt32(),
                "bool" => reader.ReadBoolean(),
                "GlbO" => new DescriptorStructure(reader, false),
                "type" => new StructureClass(reader),
                "GlbC" => new StructureClass(reader),
                "alis" => new StructureAlias(reader),
                "tdta" => new StructureUnknownOSType("Cannot read RawData"),
                "prop" => new StructureProperty(reader),
                "Clss" => new StructureClass(reader),
                "Enmr" => new StructureEnumerate(reader),
                "rele" => new StructureOffset(reader),
                "Idnt" => new StructureUnknownOSType("Cannot read Identifier"),
                "indx" => new StructureUnknownOSType("Cannot read Index"),
                "name" => new StructureUnknownOSType("Cannot read Name"),
                "ObAr" => new StructureObjectArray(reader),
                _ => throw new NotSupportedException(ostype),
                };
        }
    }

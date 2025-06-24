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

using Ntreev.Library.Psd.Exceptions;

namespace Ntreev.Library.Psd.Structures;

//internal class StructureReference(PsdBinaryReader reader) : BaseStructure(reader) { }


internal class StructureReference : Properties
    {

    public StructureReference(PsdBinaryReader reader)
        {

        var count = reader.ReadInt32();
        for (var i = 0; i < count; i++)
            {
            switch (reader.ReadAsAscii(4))
                {
                case "prop": { Add("Property", new StructureProperty(reader)); break; }
                case "Clss": { Add("Class", new StructureClass(reader)); break; }
                case "Enmr": { Add("Enumerate Reference", new StructureEnumerateReference(reader)); break; }
                case "rele": { Add("Offset", new StructureOffset(reader)); break; }
                case "idnt": { Add("Identifier", reader.ReadAsAscii(4)); break; } //BUG copy from Action
                case "indx": { Add("Index", reader.ReadInt16()); break; } // BUG guess from "Index" in Ctrl+F in document
                case "name": { Add("Name", reader.ReadString()); break; } //BUG Guess
                default:
                    throw new InvalidFormatException($"Unknown structure type");
                }
            }
        }
    }
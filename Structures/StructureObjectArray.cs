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

internal class StructureObjectArray : Properties
    {
    public StructureObjectArray(PsdBinaryReader reader)
        {
        _ = reader.ReadInt32(); //Version
        Add("Name", reader.ReadString());
        Add("ClassID", reader.ReadAsKey());

        var count = reader.ReadInt32();

        var items = new Properties[count];

        for (var i = 0; i < count; i++)
            {
            Properties props = new()
                {

                ["Type1"] = reader.ReadAsKey(),
                ["EnumName"] = reader.ReadAsType(),
                ["Type2"] = PsdUtility.ToUnitType(reader.ReadAsType()),
                ["Values"] = reader.ReadDoubles(reader.ReadInt32())
                };
            //props.Add("Type1", reader.ReadAsKey());
            //props.Add("EnumName", reader.ReadAsType());

            //props.Add("Type2", PsdUtility.ToUnitType(reader.ReadAsType()));
            //var d4 = reader.ReadInt32();
            //props.Add("Values", reader.ReadDoubles(d4));

            items[i] = props;
            }

        Add("items", items);
        }
    }


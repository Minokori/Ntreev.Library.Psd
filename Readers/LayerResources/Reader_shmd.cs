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

using Ntreev.Library.Psd.Attributes;
using Ntreev.Library.Psd.ReadersPrototype;

namespace Ntreev.Library.Psd.Readers.LayerResources;

[ResourceID("shmd")]
internal class Reader_shmd : ResourceReaderBase
    {
    public Reader_shmd(PsdBinaryReader reader, long length)
        : base(reader, length) { }

    protected override Properties ReadValue()
        {
        Properties props = [];

        var count = GlobalReader.ReadInt32();

        List<DescriptorStructure> dss = [];

        for (var i = 0; i < count; i++)
            {
            var s = GlobalReader.ReadAsAscii(4);
            var k = GlobalReader.ReadAsAscii(4);
            var c = GlobalReader.ReadByte();
            var p = GlobalReader.ReadBytes(3);
            var l = GlobalReader.ReadInt32();
            var p2 = GlobalReader.Position;
            var ds = new DescriptorStructure(GlobalReader);
            dss.Add(ds);
            GlobalReader.Position = p2 + l;
            }

        props["Items"] = dss;

        return props;
        }
    }

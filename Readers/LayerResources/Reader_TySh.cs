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

[ResourceID("TySh")]
internal class Reader_TySh : ResourceReaderBase
    {
    public Reader_TySh(PsdReader reader, long length)
        : base(reader, length) { }

    protected override IProperties ReadValue()
        {
        var props = new Properties(7);

        GlobalReader.ValidateInt16(1, "Typetool Version");
        props["Transforms"] = GlobalReader.ReadDoubles(6);
        props["TextVersion"] = GlobalReader.ReadInt16();
        props["Text"] = new DescriptorStructure(GlobalReader);
        props["WarpVersion"] = GlobalReader.ReadInt16();
        props["Warp"] = new DescriptorStructure(GlobalReader);
        props["Bounds"] = GlobalReader.ReadDoubles(2);

        return props;
        }
    }

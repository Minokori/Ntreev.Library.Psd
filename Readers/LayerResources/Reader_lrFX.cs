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

[ResourceID("lrFX")]
internal class Reader_lrFX : ResourceReaderBase
    {
    public Reader_lrFX(PsdReader reader, long length)
        : base(reader, length)
        {

        }

    protected override IProperties ReadValue()
        {
        var value = new Properties();

        var version = GlobalReader.ReadInt16();
        int count = GlobalReader.ReadInt16();

        for (var i = 0; i < count; i++)
            {
            var _8bim = GlobalReader.ReadAsAscii(4);
            var effectType = GlobalReader.ReadAsAscii(4);
            var size = GlobalReader.ReadInt32();
            var p = GlobalReader.Position;

            switch (effectType)
                {
                case "dsdw":
                    {
                    //ShadowInfo.Parse(_reader);
                    }

                break;
                case "sofi":
                    {
                    //this.solidFillInfo = SolidFillInfo.Parse(_reader);
                    }

                break;
                }

            GlobalReader.Position = p + size;
            }

        return value;
        }
    }


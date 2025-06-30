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
using Ntreev.Library.Psd.Attributes;

namespace Ntreev.Library.Psd.Readers.ResourceReader;

// (replaced by SoLd in Photoshop CS3)
[ResourceID("PlLd", DisplayName = "Placed Layer")]
internal class PlacedLayerReader(PsdBinaryReader reader, long length) : ValueReader<Properties>(reader, length, null)
    {
    protected override Properties ReadValue()
        {
        Properties props = [];

        GlobalReader.VerifySignatureIs("plcL");
        props["Version"] = GlobalReader.ReadInt32();
        props["UniqueID"] = GlobalReader.ReadAsPascalString(1);
        props["PageNumbers"] = GlobalReader.ReadInt32();
        props["Pages"] = GlobalReader.ReadInt32();
        props["AntiAlias"] = GlobalReader.ReadInt32();
        props["LayerType"] = GlobalReader.ReadInt32();

        //props["Transformation"] = GlobalReader.ReadDoubles(8);
        props["Transformation"] = new JArray(GlobalReader.ReadDoubles(8));
        GlobalReader.VerifyIntIs(0);
        props["Warp"] = new DescriptorStructure(GlobalReader);

        return props;
        }
    }

[ResourceID("SoLd", DisplayName = "PlacedLayerData")]
internal class Reader_SoLd(PsdBinaryReader reader, long length) : ValueReader<Properties>(reader, length, null)
    {
    protected override Properties ReadValue()
        {
        GlobalReader.VerifySignatureIs("soLD");
        GlobalReader.VerifyIntIs(4);
        return new DescriptorStructure(GlobalReader, true);
        }
    }
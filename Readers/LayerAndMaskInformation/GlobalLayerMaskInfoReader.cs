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

namespace Ntreev.Library.Psd.Readers.LayerAndMaskInformation;

internal class GlobalLayerMaskInfoReader(PsdBinaryReader reader) : ValueReader<JObject>(reader, true, null)
    {
    protected override long InitStreamLength() => GlobalReader.ReadInt32();

    protected override JObject ReadValue()
        {
        if (StreamLength == 0) return [];
        var filler_length = StreamLength - (2 + 8 + 2 + 1);
        var globalLayerMaskInfo = new JObject()
            {
            ["OverlayColorSpace"] = GlobalReader.ReadInt16(),
            }
        ;

        JArray colorComponents = [];

        for (var i = 0; i < 4; i++)
            {
            colorComponents.Add(GlobalReader.ReadInt16());
            }

        globalLayerMaskInfo["ColorComponents"] = colorComponents;

        globalLayerMaskInfo["Opacity"] = GlobalReader.ReadInt16();

        globalLayerMaskInfo["Kind"] = GlobalReader.ReadByte();

        globalLayerMaskInfo["Filler"] = GlobalReader.ReadBytes((int)filler_length);
        return globalLayerMaskInfo;
        }
    }

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
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Attributes;
using Ntreev.Library.Psd.ReadersPrototype;

namespace Ntreev.Library.Psd.Readers.ImageResources;

/// <summary>
/// 图像的网格和参考线信息读取器
/// </summary>
/// <param name="reader"></param>
/// <param name="length"></param>
[ResourceID("1032", DisplayName = "GridAndGuides")]
internal class GridAndGuidesReader(PsdBinaryReader reader, long length) : ResourceReaderBase(reader, length)
    {
    protected override Properties ReadValue()
        {
        Properties props = [];

        _ = GlobalReader.VerifyIntIs<int>(1); // version

        var h = GlobalReader.ReadInt32();
        props["HorizontalGrid"] = h;

        var v = GlobalReader.ReadInt32();
        props["VerticalGrid"] = v;

        var guideCount = GlobalReader.ReadInt32();

        List<int> horizontalGrids = [];
        List<int> verticalGrids = [];

        for (var i = 0; i < guideCount; i++)
            {
            var n = GlobalReader.ReadInt32();
            var t = GlobalReader.ReadByte();

            if (t == 0)
                verticalGrids.Add(n);
            else
                horizontalGrids.Add(n);
            }



        //props["HorizontalGuides"] = horizontalGrids.ToArray();
        //props["VerticalGuides"] = verticalGrids.ToArray();
        props["HorizontalGuides"] = new JArray(horizontalGrids);
        props["VerticalGuides"] = new JArray(verticalGrids);



        return props;
        }
    }

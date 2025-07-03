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
using Ntreev.Library.Psd.Interfaces;

namespace Ntreev.Library.Psd;

internal partial class PsdLayer : IPsdLayer
    {
    private PsdBinaryReader GlobalReader => Document.BinaryReader;

    public PsdLayer(JObject layerRecord, JArray channelsImageData, PsdDocument document)
        {
        Records = layerRecord;
        ChannelImageData = channelsImageData;
        Document = document;
        Channels = InitChannels();
        }

    public override string ToString() => Name;

    private Channel[] InitChannels() => Records.InitChannels(GlobalReader.Depth);

    /// <summary>
    /// 计算边距(TOP, Bottom, Left, right)
    /// </summary>
    public void ComputeBounds()
        {
        var type = Records.ToValue<string>(
            "Resources.SectionDividerSetting.SectionType",
            "Resources.lsdk.SectionType"
        );
        var sectionType = string.IsNullOrEmpty(type)
            ? SectionType.Normal
            : Enum.Parse<SectionType>(type);
        if (sectionType is not SectionType.Open and not SectionType.Closed)
            {
            return;
            }

        var left = int.MaxValue;
        var top = int.MaxValue;
        var right = int.MinValue;
        var bottom = int.MinValue;

        var isSet = false;

        foreach (var item in this.Descendants())
            {
            if (item == this || item.HasImage == false)
                continue;

            if (item.Records.Contains("Resources.PlacedLayer.Transformation"))
                {
                var transforms = item
                    .Records.SelectToken("Resources.PlacedLayer.Transformation")
                    .ToObject<double[]>()!; // ToValue<double[]>("PlLd", "Transformation");
                double[] xx = [transforms[0], transforms[2], transforms[4], transforms[6]];
                double[] yy = [transforms[1], transforms[3], transforms[5], transforms[7]];

                var l = (int)Math.Ceiling(xx.Min());
                var r = (int)Math.Ceiling(xx.Max());
                var t = (int)Math.Ceiling(yy.Min());
                var b = (int)Math.Ceiling(yy.Max());
                left = Math.Min(l, left);
                top = Math.Min(t, top);
                right = Math.Max(r, right);
                bottom = Math.Max(b, bottom);
                }
            else
                {
                left = Math.Min(item.Left, left);
                top = Math.Min(item.Top, top);
                right = Math.Max(item.Right, right);
                bottom = Math.Max(item.Bottom, bottom);
                }

            isSet = true;
            }

        if (isSet == false)
            return;

        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
        }

    /// <summary>
    /// 递归遍历指定 <see cref="PsdLayer"/> 及其所有子层，返回包含自身及所有后代层的枚举序列。
    /// </summary>
    /// <param name="layer">要遍历的根 <see cref="PsdLayer"/> 实例。</param>
    /// <returns>包含自身及所有后代 <see cref="PsdLayer"/> 的 <see cref="IEnumerable{PsdLayer}"/> 序列。</returns>
    internal IEnumerable<PsdLayer> Descendants()
        {
        yield return this;
        foreach (var item in Childs)
            {
            foreach (var child in item.Descendants())
                {
                yield return child;
                }
            }
        }
    }

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

namespace Ntreev.Library.Psd;

internal partial class PsdLayer : IPsdLayer
    {
    private PsdBinaryReader GlobalReader => Document.BinaryReader;

    private static readonly PsdLayer[] _emptyChilds = [];

    public PsdLayer(JObject layRecord, JArray channelsImageData, PsdDocument document)
        {
        Records = layRecord;
        ChannelImageData = channelsImageData;
        // 根据 layer record 初始化 PSD Layer

        Document = document;
        Channels = InitChannels();
        }
    public override string ToString() => Name;

    private Channel[] InitChannels()
        {
        var channels = Records.InitChannels(GlobalReader.Depth);

        foreach (var item in channels.Zip(ChannelImageData.Children(), (channel, metainfo) => new { Channel = channel, MetaInfo = (JObject)metainfo }))
            {
            item.Channel.MetaInfo = item.MetaInfo;
            }

        return channels;
        }


    /// <summary>
    /// 计算边距(TOP, Bottom, Left, right)
    /// </summary>
    public void ComputeBounds()
        {
        var type = Records.ToValue<string>("Resources.lsct.SectionType", "Resources.lsdk.SectionType");
        var sectionType = string.IsNullOrEmpty(type) ? SectionType.Normal : Enum.Parse<SectionType>(type);

        if (sectionType is not SectionType.Opend and not SectionType.Closed)
            return;

        var left = int.MaxValue;
        var top = int.MaxValue;
        var right = int.MinValue;
        var bottom = int.MinValue;

        var isSet = false;

        foreach (var item in this.Descendants())
            {
            if (item == this || item.HasImage == false)
                continue;

            if (item.Resources.Contains("PlLd.Transformation"))
                {
                var transforms = item.Resources.SelectToken("PlLd.Transformation").ToObject<double[]>()!;// ToValue<double[]>("PlLd", "Transformation");
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
    }

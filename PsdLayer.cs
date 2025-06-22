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


using Ntreev.Library.Psd.Readers.LayerAndMaskInformation;

namespace Ntreev.Library.Psd;

partial class PsdLayer : IPsdLayer
    {
    private readonly PsdDocument _document;
    private readonly LayerRecords _records;

    private int _left, _top, _right, _bottom;

    private PsdLayer[] _childs = [];
    private PsdLayer _parent;
    private ILinkedLayer _linkedLayer;

    private ChannelsReader _channelsReader;

    private static readonly PsdLayer[] _emptyChilds = [];

    public PsdLayer(PsdBinaryReader reader, PsdDocument document)
        {
        _document = document;
        _records = LayerRecordsReader.Read(reader);
        _records = LayerExtraRecordsReader.Read(reader, this._records);

        _left = _records.Left;
        _top = _records.Top;
        _right = _records.Right;
        _bottom = _records.Bottom;
        }

    public override string ToString()
        {
        return this.Name;
        }


    public void ReadChannels(PsdBinaryReader reader)
        {
        this._channelsReader = new ChannelsReader(reader, this._records.ChannelSize, this);
        }


    /// <summary>
    /// 计算边距
    /// </summary>
    public void ComputeBounds()
        {
        SectionType sectionType = this._records.SectionType;
        if (sectionType != SectionType.Opend && sectionType != SectionType.Closed)
            return;

        int left = int.MaxValue;
        int top = int.MaxValue;
        int right = int.MinValue;
        int bottom = int.MinValue;

        bool isSet = false;

        foreach (var item in this.Descendants())
            {
            if (item == this || item.HasImage == false)
                continue;

            // 일반 레이어인데 비어 있을때
            if (item.Resources.Contains("PlLd.Transformation"))
                {
                double[] transforms = (double[])item.Resources["PlLd.Transformation"];
                double[] xx = [transforms[0], transforms[2], transforms[4], transforms[6],];
                double[] yy = [transforms[1], transforms[3], transforms[5], transforms[7],];

                int l = (int)Math.Ceiling(xx.Min());
                int r = (int)Math.Ceiling(xx.Max());
                int t = (int)Math.Ceiling(yy.Min());
                int b = (int)Math.Ceiling(yy.Max());
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

        this._left = left;
        this._top = top;
        this._right = right;
        this._bottom = bottom;
        }


    }



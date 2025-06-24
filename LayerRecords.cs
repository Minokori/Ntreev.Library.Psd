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

namespace Ntreev.Library.Psd;

internal class LayerRecords
    {
    private LayerBlendingRanges blendingRanges;
    private string name;
    private int version;

    public void SetExtraRecords(LayerMask layerMask, LayerBlendingRanges blendingRanges, IProperties resources, string name)
        {
        this.Mask = layerMask;
        this.blendingRanges = blendingRanges;
        this.Resources = resources;
        this.name = name;

        this.Resources.TryGetValue<string>(ref this.name, "luni.Name");
        this.Resources.TryGetValue<int>(ref this.version, "lyvr.Version");
        if (this.Resources.Contains("lsct.SectionType") == true)
            this.SectionType = (SectionType)this.Resources.ToInt32("lsct.SectionType");
        if (this.Resources.Contains("lsdk.SectionType") == true)
            this.SectionType = (SectionType)this.Resources.ToInt32("lsdk.SectionType");

        if (this.Resources.Contains("SoLd.Idnt") == true)
            this.PlacedID = this.Resources.ToGuid("SoLd.Idnt");
        else if (this.Resources.Contains("SoLE.Idnt") == true)
            this.PlacedID = this.Resources.ToGuid("SoLE.Idnt");

        foreach (var item in this.Channels)
            {
            switch (item.Type)
                {
                case ChannelType.Mask:
                    {
                    if (this.Mask != null)
                        {
                        item.Width = this.Mask.Width;
                        item.Height = this.Mask.Height;
                        }
                    }

                break;
                case ChannelType.Alpha:
                    {
                    if (this.Resources.Contains("iOpa") == true)
                        {
                        var opa = this.Resources.ToByte("iOpa", "Opacity");
                        item.Opacity = opa / 255.0f;
                        }
                    }

                break;
                }
            }
        }

    public void ValidateSize()
        {
        var width = this.Right - Left;
        var height = this.Bottom - this.Top;

        if ((width > 0x3000) || (height > 0x3000))
            {
            throw new NotSupportedException(string.Format("Invalidated size ({0}, {1})", width, height));
            }
        }

    public int Left { get; set; }

    public int Top { get; set; }

    public int Right { get; set; }

    public int Bottom { get; set; }

    public int Width => this.Right - this.Left;

    public int Height => this.Bottom - this.Top;

    public int ChannelCount
        {
        get => this.Channels == null ? 0 : Channels.Length;
        set
            {
            if (value > 0x38)
                {
                throw new Exception(string.Format("Too many channels : {0}", value));
                }

            this.Channels = new Channel[value];
            for (var i = 0; i < value; i++)
                {
                this.Channels[i] = new Channel();
                }
            }
        }

    public Channel[] Channels { get; private set; }

    public BlendMode BlendMode { get; set; }

    public byte Opacity { get; set; }

    public bool Clipping { get; set; }

    public LayerFlags Flags { get; set; }

    public int Filter { get; set; }

    public long ChannelSize => this.Channels.Select(item => item.Size).Aggregate((v, n) => v + n);

    public SectionType SectionType { get; private set; }

    public Guid PlacedID { get; private set; }

    public string Name => this.name;

    public LayerMask Mask { get; private set; }

    public object BlendingRanges => this.blendingRanges;

    public IProperties Resources { get; private set; }

    public int Version => this.version;
    }

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

internal partial class LayerRecords : Properties
    {
    public void AddRangeRecords(Properties resources)
        {


        this.Resources = resources;
        var n = this.Resources.SelectToken("luni.Name")?.ToObject<string>();
        if (n is not null)
            Name = n;
        var v = this.Resources.SelectToken("lyvr.Version")?.ToObject<int>();
        if (v is not null)
            Version = v.Value;
        if (this.Resources.Contains("lsct.SectionType") == true)
            this.SectionType = Enum.Parse<SectionType>(
                this.Resources.SelectToken("lsct.SectionType").ToObject<string>()
            );
        if (this.Resources.Contains("lsdk.SectionType") == true)
            this.SectionType = Enum.Parse<SectionType>(
                this.Resources.SelectToken("lsdk.SectionType").ToObject<string>()
            );

        if (this.Resources.Contains("SoLd.Idnt") == true)
            this.PlacedID = new(this.Resources.SelectToken("SoLd.Idnt").ToObject<string>());
        else if (this.Resources.Contains("SoLE.Idnt") == true)
            this.PlacedID = new(this.Resources.SelectToken("SoLE.Idnt").ToObject<string>());

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
                        //var opa = this.Resources.ToByte("iOpa", "Opacity");
                        var opa = this.Resources.SelectToken("iOpa.Opacity").ToObject<byte>();
                        item.Opacity = opa / 255.0f;
                        }
                    }

                break;
                }
            }
        }

    public void ValidateSize()
        {
        if ((Width > 0x3000) || (Height > 0x3000))
            {
            throw new NotSupportedException($"Invalidated size ({Width}, {Height})");
            }
        }
    }

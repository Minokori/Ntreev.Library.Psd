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

internal partial class LayerRecords
    {
    private string name;
    private int version;

    public void NewSetExtraRecords(Properties resources)
        {
        this.Resources = resources;
        this.Resources.TryGetValue<string>(ref this.name, "luni.Name");
        this.Resources.TryGetValue<int>(ref this.version, "lyvr.Version");
        if (this.Resources.Contains("lsct.SectionType") == true)
            this.SectionType = Enum.Parse<SectionType>(this.Resources.ToString("lsct.SectionType"));
        if (this.Resources.Contains("lsdk.SectionType") == true)
            this.SectionType = Enum.Parse<SectionType>(this.Resources.ToString("lsdk.SectionType"));

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
        if ((Width > 0x3000) || (Height > 0x3000))
            {
            throw new NotSupportedException($"Invalidated size ({Width}, {Height})");
            }
        }

    }

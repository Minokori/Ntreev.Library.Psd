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

internal class LayerExtraRecords
    {
    private readonly LayerMaskReader layerMask;
    private readonly LayerBlendingRangesReader blendingRanges;
    private readonly LayerResourceReader resources;
    private readonly string name;
    private readonly SectionType sectionType;

    public LayerExtraRecords(
        LayerMaskReader layerMask,
        LayerBlendingRangesReader blendingRanges,
        LayerResourceReader resources,
        string name
    )
        {
        this.layerMask = layerMask;
        this.blendingRanges = blendingRanges;
        this.resources = resources;
        this.name = name;

        this.resources.TryGetValue<string>(ref this.name, "luni.Name");
        this.resources.TryGetValue<SectionType>(ref this.sectionType, "lsct.SectionType");

        if (this.resources.Contains("SoLd.Idnt") == true)
            this.PlacedID = this.resources.ToGuid("SoLd.Idnt");
        else if (this.resources.Contains("SoLE.Idnt") == true)
            this.PlacedID = this.resources.ToGuid("SoLE.Idnt");
        }

    public SectionType SectionType => this.sectionType;

    public Guid PlacedID { get; }

    public string Name => this.name;

    public LayerMask Mask => this.layerMask.Value;

    public object BlendingRanges => this.blendingRanges.Value;

    public Properties Resources => this.resources.Value;
    }

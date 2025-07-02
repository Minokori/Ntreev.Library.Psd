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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Interfaces;
namespace Ntreev.Library.Psd.Sections;

internal partial class LayerAndMaskInformationSection : JObject
    {
    public LayerAndMaskInformationSection(
        JObject layerInfo,
        JObject globalLayerMask,
        JObject additionalLayerInfo
    )
        {
        this[nameof(LayerInformation)] = layerInfo;
        this[nameof(GlobalLayerMask)] = globalLayerMask;
        this[nameof(AdditionalLayerInformation)] = additionalLayerInfo;
        }


    public JObject LayerInformation => this.ToValue<JObject>("LayerInformation")!;
    public JObject GlobalLayerMask => this.ToValue<JObject>("GlobalLayerMask")!;
    public JObject AdditionalLayerInformation => this.ToValue<JObject>("AdditionalLayerInformation")!;


    [JsonIgnore]
    public PsdDocument? Document { get; init; }

    [JsonIgnore]
    public PsdLayer[] Layers
        {
        get
            {
            field ??= InitPsdLayers();
            return field;
            }
        }

    [JsonIgnore]
    public ILinkedLayer[] LinkedLayers { get; init; } = [];
    }

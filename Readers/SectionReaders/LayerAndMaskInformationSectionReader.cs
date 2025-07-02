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
using Ntreev.Library.Psd.Readers.LayerAndMaskInformation;
using Ntreev.Library.Psd.Sections;

namespace Ntreev.Library.Psd.Readers;

internal class LayerAndMaskInformationSectionReader(PsdBinaryReader reader, PsdDocument document)
    : ValueReader<LayerAndMaskInformationSection>(reader, true, document)
    {
    protected override LayerAndMaskInformationSection ReadValue()
        {
        #region Layer Info
        var layerInfoReader = new LayerInfoReader(GlobalReader);
        var layerInfo = layerInfoReader.Value;
        #endregion


        #region Global Layer Mask Info
        JObject globalLayerMaskInfo = [];
        if (GlobalReader.Position + 4 < EndPosition)
            {
            var globalLayerMaskInfoReader = new GlobalLayerMaskInfoReader(GlobalReader);
            globalLayerMaskInfo = globalLayerMaskInfoReader.Value;
            }
        #endregion


        #region Additional Info (JObeject) & Linked/Embedded Layers (ILinkedLayer)
        JObject additionalInfo = [];
        LinkedLayer[] linkedLayers = [];
        EmbeddedLayer[] embeddedLayers = [];
        if (GlobalReader.Position + 4 < EndPosition)
            {
            var additionalInfoReader = new DocumentResourceReader(
                GlobalReader,
                EndPosition - GlobalReader.Position
            );
            (additionalInfo, linkedLayers, embeddedLayers) = additionalInfoReader.Value;
            }
        #endregion

        return new(layerInfo, globalLayerMaskInfo, additionalInfo)
            {
            Document = (PsdDocument)UserData,
            LinkedLayers = [.. linkedLayers.Cast<ILinkedLayer>(), .. embeddedLayers.Cast<ILinkedLayer>()],
            };
        }
    }

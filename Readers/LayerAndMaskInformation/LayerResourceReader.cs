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

namespace Ntreev.Library.Psd.Readers.LayerAndMaskInformation;

internal class LayerResourceReader(PsdBinaryReader reader, long length)
    : ValueReader<JObject>(reader, length, null)
    {
    protected override JObject ReadValue()
        {
        JObject props = [];

        while (GlobalReader.Position < EndPosition)
            {
            _ = GlobalReader.VerifySignatureIs("8BIM");
            var resourceID = GlobalReader.ReadAsType();
            long length = GlobalReader.ReadInt32().PadToEven();

            var resourceReader = ReaderCollector.CreateReader(resourceID, GlobalReader, length);
            var resourceName = ReaderCollector.GetDisplayName(resourceID);

            props[resourceName] = resourceReader.Value;
            }

        return props;
        }
    }

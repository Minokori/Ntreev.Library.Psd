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

internal class DocumentResourceReader(PsdBinaryReader reader, long length)
    : ValueReader<Tuple<JObject, LinkedLayer[], EmbeddedLayer[]>>(reader, length, null)
    {
    private static readonly string[] doubleTypeKeys =
    [
        "LMsk",
        "Lr16",
        "Lr32",
        "Layr",
        "Mt16",
        "Mt32",
        "Mtrn",
        "Alph",
        "FMsk",
        "lnk2",
        "FEid",
        "FXid",
        "PxSD",
        "lnkE",
        "extd",
    ];

    protected override Tuple<JObject, LinkedLayer[], EmbeddedLayer[]> ReadValue()
        {
        JObject props = [];
        List<LinkedLayer> linkedLayers = [];
        List<EmbeddedLayer> embeddedLayers = [];
        while (GlobalReader.Position < EndPosition)
            {
            _ = GlobalReader.VerifySignatureIs("8BIM", "8B64");
            var resourceID = GlobalReader.ReadAsType();
            var length = ReadLength(GlobalReader, resourceID);

            var resource = ReaderCollector.CreateReader(resourceID, GlobalReader, length).Value;
            var resourceName = ReaderCollector.GetDisplayName(resourceID);

            switch (resourceName)
                {
                case "LinkedLayer":
                    {
                    var items = (JArray)resource;
                    foreach (var item in items)
                        {
                        var linkedLayer = new LinkedLayer((JObject)item);
                        linkedLayers.Add(linkedLayer);
                        }

                    props[resourceName] = items;
                    continue;
                    }
                case "EmbeddedLayer":
                    {
                    var items = (JArray)resource;
                    foreach (var item in items)
                        {
                        var embeddedLayer = new EmbeddedLayer((JObject)item);
                        embeddedLayers.Add(embeddedLayer);
                        }

                    props[resourceName] = items;
                    continue;
                    }
                default:
                    {
                    props[resourceName] = resource;
                    continue;
                    }
                }
            }

        return new(props, linkedLayers.ToArray(), embeddedLayers.ToArray());
        }

    private static long ReadLength(PsdBinaryReader reader, string resourceID)
        {
        var length =
            doubleTypeKeys.Contains(resourceID) && reader.Version == 2 ? reader.ReadInt64()
            : reader.Version == 2 ? reader.ReadInt64()
            : reader.ReadInt32();
        return length.PadToFour();
        }
    }

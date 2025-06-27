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
using Ntreev.Library.Psd.Readers.LayerAndMaskInformation;

namespace Ntreev.Library.Psd.Readers;

internal class LayerAndMaskInformationSectionReader(PsdBinaryReader reader, PsdDocument document)
    : ValueReader<LayerAndMaskInformationSection>(reader, true, document)
    {
    protected override LayerAndMaskInformationSection ReadValue()
        {
        // TODO LayerInfo 的 ImageChannelData 还没有读取
        var layerInfo = new LayerInfoReader(GlobalReader).Value;


        //globalLayerMaskInfo
        // addtionalInfo


        //把 ChannelsImageData 移出来
        var layers = InitPsdLayers(layerInfo);

        // 在这里初始化 PSD Layers

        //下面是读取 GlobalLayerMaskInfo
        //var globalMask = new GlobalLayerMaskInfoReader(GlobalReader).Value;
        if (GlobalReader.Position + 4 >= EndPosition)
            {
            return new LayerAndMaskInformationSection(layerInfo, null, []) { Layers = layers };
            }
        else
            {
            //BUG
            GlobalLayerMaskInfoReader globalLayerMask = new(GlobalReader);
            DocumentResourceReader documentResource = new(
                GlobalReader,
                EndPosition - GlobalReader.Position
            );
            return new LayerAndMaskInformationSection(
                layerInfo,
                globalLayerMask,
                documentResource.Value
            )
                {
                Layers = layers,
                };
            }
        }

    private PsdLayer[] InitPsdLayers(JObject layerInfo)
        {
        // 用 layer record初始化 PSD Layers
        var layers = new PsdLayer[layerInfo.ToValue<int>("LayerCount")];
        for (var i = 0; i < layers.Length; i++)
            {
            layers[i] = new PsdLayer(GlobalReader, (PsdDocument)UserData)
                {
                Records = (JObject)layerInfo.SelectToken($"LayerRecord[{i}]")!,
                };
            }

        // 初始化每个 PsdLayer 的 ChannelReader,
        foreach (var layer in layers)
            {
            layer.InitChannelReader(GlobalReader);
            }

        // 计算每个 PsdLayer 的 父子关系
        layers = Initialize(null, layers);


        // 计算每个 PsdLayer 的边距
        foreach (var item in layers.SelectMany(item => item.Descendants()).Reverse())
            {
            item.ComputeBounds();
            }

        return layers;
        }


    /// <summary>
    /// 计算 PSD Layers 的层级关系
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="layers"></param>
    /// <returns></returns>
    private static PsdLayer[] Initialize(PsdLayer parent, PsdLayer[] layers)
        {
        Stack<PsdLayer> stack = new();
        List<PsdLayer> rootLayers = [];
        Dictionary<PsdLayer, List<PsdLayer>> layerToChilds = [];

        foreach (var layer in ((IEnumerable<PsdLayer>)layers).Reverse())
            {
            if (layer.SectionType == SectionType.Divider)
                {
                parent = stack.Pop();
                continue;
                }

            if (parent != null)
                {
                if (layerToChilds.ContainsKey(parent) == false)
                    {
                    layerToChilds.Add(parent, []);
                    }

                var childs = layerToChilds[parent];
                childs.Insert(0, layer);
                layer.Parent = parent;
                }
            else
                {
                rootLayers.Insert(0, layer);
                }

            if (layer.SectionType is SectionType.Opend or SectionType.Closed)
                {
                stack.Push(parent);
                parent = layer;
                }
            }

        foreach (var item in layerToChilds)
            {
            item.Key.Childs = [.. item.Value];
            }

        return [.. rootLayers];
        }
    }

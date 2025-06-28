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

namespace Ntreev.Library.Psd;

internal class LayerAndMaskInformationSection(JObject layerInfo, GlobalLayerMaskInfoReader globalLayerMask, Properties documentResources)
    {
    public JObject LayerInfo { get; init; } = layerInfo;
    public GlobalLayerMaskInfoReader GlobalLayerMask { get; init; } = globalLayerMask;

    public PsdDocument Document { get; init; }

    public PsdLayer[] Layers
        {
        get
            {
            field ??= InitPsdLayers();
            return field;
            }
        }

    public ILinkedLayer[] LinkedLayers
        {
        get
            {
            if (field == null)
                {
                List<ILinkedLayer> list = [];
                string[] ids = ["lnk2", "lnk3", "lnkD", "lnkE",];

                foreach (var item in ids)
                    {
                    if (this.Resources.Contains(item))
                        {
                        //var items = this.Resources.ToValue<ILinkedLayer[]>(item, "Items");
                        var items = this.Resources.LinkedLayers;
                        list.AddRange(items);
                        }
                    }

                field = [.. list];
                }

            return field;
            }
        }

    public Properties Resources { get; } = documentResources;

    private PsdLayer[] InitPsdLayers()
        {
        // 用 layer record初始化 PSD Layers
        var layers = new PsdLayer[LayerInfo.ToValue<int>("LayerCount")];
        for (var i = 0; i < layers.Length; i++)
            {
            var layerRecord = LayerInfo.SelectToken($"LayerRecord[{i}]") as JObject;
            var channelsImageData = LayerInfo.SelectToken($"ChannelsImageData[{i}]") as JArray;

            layers[i] = new PsdLayer(layerRecord, channelsImageData, Document);

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


using Newtonsoft.Json.Linq;

namespace Ntreev.Library.Psd.Sections;
internal partial class LayerAndMaskInformationSection
    {
    private PsdLayer[] InitPsdLayers()
        {
        // 用 layer record初始化 PSD Layers
        var layers = new PsdLayer[LayerInformation.ToValue<int>("LayerCount")];
        for (var i = 0; i < layers.Length; i++)
            {
            var layerRecord = LayerInformation.SelectToken($"LayerRecord[{i}]") as JObject;
            var channelsImageData = LayerInformation.SelectToken($"ChannelsImageData[{i}]") as JArray;
            layers[i] = new PsdLayer(layerRecord, channelsImageData, Document);
            }

        // 计算每个 PsdLayer 的 父子关系
        layers = Initialize(layers);

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
    private static PsdLayer[] Initialize(PsdLayer[] layers, PsdLayer? parent = null)
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
                    layerToChilds.Add(parent, []);

                var children = layerToChilds[parent];
                children.Insert(0, layer);
                layer.Parent = parent;
                }
            else
                {
                rootLayers.Insert(0, layer);
                }

            if (layer.SectionType is SectionType.Open or SectionType.Closed)
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

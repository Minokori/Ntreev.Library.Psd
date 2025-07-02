using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Attributes;

namespace Ntreev.Library.Psd.Readers.ResourceReader;
// TODO lrFX, EffectsLayer 现在不读取任何有用信息
[ResourceID("lrFX", DisplayName = "EffectsLayer")]
internal class EffectsLayerReader(PsdBinaryReader reader, long length)
    : ValueReader<JToken>(reader, length, null)
    {
    protected override JObject ReadValue()
        {
        JObject value = [];

        var version = GlobalReader.ReadInt16();
        int count = GlobalReader.ReadInt16();

        for (var i = 0; i < count; i++)
            {
            var _8bim = GlobalReader.ReadAsAscii(4);
            var effectType = GlobalReader.ReadAsAscii(4);
            var size = GlobalReader.ReadInt32();
            var p = GlobalReader.Position;

            switch (effectType)
                {
                case "dsdw":
                    {
                    //ShadowInfo.Parse(_reader);
                    }

                break;
                case "sofi":
                    {
                    //this.solidFillInfo = SolidFillInfo.Parse(_reader);
                    }

                break;
                }

            GlobalReader.Position = p + size;
            }

        return value;
        }
    }

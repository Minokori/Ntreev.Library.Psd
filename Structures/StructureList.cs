using Newtonsoft.Json.Linq;

namespace Ntreev.Library.Psd.Structures;

// TODO : 需要重构, 目前的实现方式不符合预期
internal class StructureList : Properties
    {
    public StructureList(PsdBinaryReader reader)
        {
        List<object> items = [];
        var count = reader.ReadInt32();
        for (var i = 0; i < count; i++)
            {
            var type = reader.ReadAsType();
            var value = StructureReader.Read(type, reader);
            items.Add(value);
            }

        Add("Items", new JArray(items));
        }
    }


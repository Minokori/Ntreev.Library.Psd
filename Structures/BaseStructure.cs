namespace Ntreev.Library.Psd.Structures;

internal class BaseStructure : Properties
    {
    public BaseStructure(PsdReader reader)
        {
        List<object> items = [];
        var count = reader.ReadInt32();
        for (var i = 0; i < count; i++)
            {
            var type = reader.ReadAsType();
            var value = StructureReader.Read(type, reader);
            items.Add(value);
            }

        Add("Items", items.ToArray());
        }
    }

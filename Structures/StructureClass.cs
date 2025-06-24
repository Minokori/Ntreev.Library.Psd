namespace Ntreev.Library.Psd.Structures;

internal class StructureClass : Properties
    {
    public StructureClass(PsdBinaryReader reader)
        : base(2)
        {
        Add("Name", reader.ReadString());
        Add("ClassID", reader.ReadAsKey());
        }
    }

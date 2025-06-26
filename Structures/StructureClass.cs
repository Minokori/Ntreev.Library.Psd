namespace Ntreev.Library.Psd.Structures;

internal class StructureClass : Properties
    {
    public StructureClass(PsdBinaryReader reader)
        : base()
        {
        Add("Name", reader.ReadString());
        Add("ClassID", reader.ReadAsKey());
        }
    }

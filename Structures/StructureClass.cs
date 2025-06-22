
namespace Ntreev.Library.Psd.Structures;

internal class StructureClass : Properties
    {
    public StructureClass() : base(2)
        {

        }

    public StructureClass(PsdBinaryReader reader)
        {
        Add("Name", reader.ReadString());
        Add("ClassID", reader.ReadAsKey());
        }
    }


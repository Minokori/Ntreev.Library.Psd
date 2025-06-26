using Ntreev.Library.Psd.Exceptions;

namespace Ntreev.Library.Psd.Readers;

internal class FileHeaderSectionReader(PsdBinaryReader reader)
    : ValueReader<FileHeaderSection>(reader, 26, null)
    {
    public static FileHeaderSection Read(PsdBinaryReader reader)
        {
        FileHeaderSectionReader instance = new(reader);
        return instance.Value;
        }

    protected override FileHeaderSection ReadValue()
        {
        var value = new FileHeaderSection
            {
            ["Signature"] = GlobalReader.ReadAsType(),
            ["Version"] = GlobalReader.ReadInt16(),
            ["Reserved"] = GlobalReader.ReadBytes(6).Sum(b => b),
            ["NumberOfChannels"] = GlobalReader.ReadInt16(),
            ["Height"] = GlobalReader.ReadInt32(),
            ["Width"] = GlobalReader.ReadInt32(),
            ["Depth"] = GlobalReader.ReadInt16(),
            ["ColorMode"] = Enum.GetName(GlobalReader.ReadAsColorMode()),
            };

        return value["Reserved"]!.ToObject<int>() != 0
            ? throw new InvalidFormatException("Reserved bytes in PSD file header must be zero.")
            : value["Signature"]!.ToObject<string>() != "8BPS"
            ? throw new InvalidFormatException("Invalid PSD file signature. Expected '8BPS'.")
            : value["Depth"]!.ToObject<int>() != 8
            ? throw new NotSupportedException("only support 8 Bit Channel")
            : value;
        }
    }

using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Exceptions;

namespace Ntreev.Library.Psd.Structures;

internal partial class StructureReader //如何读取 不同的数据结构
    {
    #region 基本数据类型, 返回 JValue

    private static JValue ReadDouble(PsdBinaryReader reader) => new(reader.ReadDouble());

    private static JValue ReadString(PsdBinaryReader reader) => new(reader.ReadString());

    private static JValue ReadInt32(PsdBinaryReader reader) => new(reader.ReadInt32());

    private static JValue ReadBoolean(PsdBinaryReader reader) => new(reader.ReadBoolean());

    private static JValue ReadInt64(PsdBinaryReader reader) => new(reader.ReadInt64());
    #endregion

    #region 简单数据类型 (没有嵌套包含其他简单类型), 返回 JObject
    private static JObject ReadProperty(PsdBinaryReader reader) =>
        new()
            {
            ["Name"] = reader.ReadString(),
            ["ClassID"] = reader.ReadAsKey(),
            ["KeyID"] = reader.ReadAsKey(),
            };

    private static JObject ReadUnitFloat(PsdBinaryReader reader) =>
        new()
            {
            ["Type"] = Enum.GetName(PsdUtility.ToUnitType(reader.ReadAsType())),
            ["Value"] = reader.ReadDouble(),
            };

    private static JObject ReadClass(PsdBinaryReader reader) =>
        new() { ["Name"] = reader.ReadString(), ["ClassID"] = reader.ReadAsKey() };

    private static JObject ReadEnumerate(PsdBinaryReader reader) =>
        new() { ["Type"] = reader.ReadAsKey(), ["Enum"] = reader.ReadAsKey() };

    private static JObject ReadEnumerateReference(PsdBinaryReader reader) =>
        new()
            {
            ["Name"] = reader.ReadString(),
            ["ClassID"] = reader.ReadAsKey(),
            ["TypeID"] = reader.ReadAsKey(),
            ["EnumID"] = reader.ReadAsKey(),
            };

    private static JObject ReadAlias(PsdBinaryReader reader) =>
        new() { ["Alias"] = reader.ReadAsAscii(reader.ReadInt32()) };

    private static JObject ReadOffset(PsdBinaryReader reader) =>
        new()
            {
            ["Name"] = reader.ReadString(),
            ["ClassID"] = reader.ReadAsKey(),
            ["Offset"] = reader.ReadInt32(),
            };


    private static JObject ReadEngineData(PsdBinaryReader reader)
        {
            {
            var length = reader.ReadInt32();
            var content = reader.ReadBytes(length);
            var result = SplitByUtf16BeBlocks(content);
            var text = "";
            foreach (var block in result)
                {
                if (block[0] == 0xFE && block[1] == 0xFF && block[^1] == 10 && block[^2] == 41)
                    {
                    var rawString = Encoding.BigEndianUnicode.GetString(block, 2, block.Length - 4);
                    if (rawString.StartsWith("、。，．・：；？！ー―’”）〕］｝〉》」』】"))
                        {
                        StringBuilder sb = new();
                        // TODO /NoStart 后的字符串报错
                        foreach (var c in rawString)
                            {
                            sb.AppendFormat("\\u{0:x4}", (int)c);
                            }

                        rawString = sb.ToString();
                        }

                    text += rawString;
                    text = text.TrimEnd('\r', '\n');
                    text += Encoding.ASCII.GetString(block[^2..]);
                    }
                else
                    {
                    text += Encoding.ASCII.GetString(block);
                    }
                }

            return ParseToJson(text);
            }

        }
    #endregion

    #region 复杂数据类型 (嵌套包含简单类型或其他复杂数据类型, 返回 JObject)
    private static JObject ReadReference(PsdBinaryReader reader)
        {
        JObject reference = [];
        var count = reader.ReadInt32();
        for (var i = 0; i < count; i++)
            {
            switch (reader.ReadAsAscii(4))
                {
                case "prop":
                    {
                    reference.Add("Property", ReadReference(reader));
                    break;
                    }
                case "Clss":
                    {
                    reference.Add("Class", ReadClass(reader));
                    break;
                    }
                case "Enmr":
                    {
                    reference.Add("Enumerate Reference", ReadEnumerateReference(reader));
                    break;
                    }
                case "rele":
                    {
                    reference.Add("Offset", ReadOffset(reader));
                    break;
                    }
                case "idnt":
                    {
                    reference.Add("Identifier", reader.ReadAsAscii(4));
                    break;
                    } //BUG copy from Action
                case "indx":
                    {
                    reference.Add("Index", reader.ReadInt16());
                    break;
                    } // BUG guess from "Index" in Ctrl+F in document
                case "name":
                    {
                    reference.Add("Name", reader.ReadString());
                    break;
                    } //BUG Guess
                default:
                    throw new InvalidFormatException($"Unknown structure type");
                }
            }

        return reference;
        }

    #endregion

    #region 会造成递归的复杂数据类型 (嵌套包含简单类型或其他复杂数据类型, 返回 JArray/JObject)
    private static JArray ReadStructureList(PsdBinaryReader reader)
        {
        JArray list = [];
        var count = reader.ReadInt32();
        for (var i = 0; i < count; i++)
            {
            var type = reader.ReadAsType();
            var value = Read(type, reader);
            list.Add(value);
            }

        return list;
        }

    private static JObject ReadSubDescriptor(PsdBinaryReader reader)
        {
        // no version to read
        JObject obj = [];
        obj.Add("Name", reader.ReadString());
        obj.Add("ClassID", reader.ReadAsKey());

        var count = reader.ReadInt32();
        for (var i = 0; i < count; i++)
            {
            // key, and osType, both 4 bytes
            var key = reader.ReadAsKey();
            var osType = reader.ReadAsType();
            var prop = Read(osType, reader);
            obj.Add(key.Trim(), prop);
            }

        return obj;
        }

    #endregion

    #region 暂时不受支持的(原版程序中没有实现)
    private static JObject ReadObjectArray(PsdBinaryReader reader)
        {
        _ = reader.ReadInt32(); //Version
        JObject objectArray = [];
        objectArray.Add("Name", reader.ReadString());
        objectArray.Add("ClassID", reader.ReadAsKey());

        var count = reader.ReadInt32();

        JArray items = [];

        for (var i = 0; i < count; i++)
            {
            JObject props = new()
                {
                ["Type1"] = reader.ReadAsKey(),
                ["EnumName"] = reader.ReadAsType(),
                ["Type2"] = Enum.GetName(PsdUtility.ToUnitType(reader.ReadAsType())),
                ["Values"] = new JArray(reader.ReadDoubles(reader.ReadInt32())),
                };

            items.Add(props);
            }

        objectArray.Add("Items", items);
        return objectArray;
        }
    #endregion


    private static JObject ParseToJson(string text)
        {
        // 2. 简单替换结构为JSON格式
        text = Regex.Replace(text, @"/([A-z]*)\r", m => $"\"{m.Groups[1].Value}\":\n");
        text = Regex.Replace(text, @"/([A-z]*)\n", m => $"\"{m.Groups[1].Value}\":\n");
        text = Regex.Replace(text, @"<<", "{");
        text = Regex.Replace(text, @">>", "},");
        text = Regex.Replace(text, @"/([A-z\d]*) ", m => $"\"{m.Groups[1].Value}\":");
        text = Regex.Replace(text, @"\(([\s\S]*?)\)", m => $"\"{m.Groups[1].Value}\",");
        text = Regex.Replace(
            text,
            @"\[ ([\d| |\.]*?) \]",
            m => $"[{m.Groups[1].Value.Replace(" ", ",")}]"
        );
        text = Regex.Replace(text, @"(\])", m => $"{m.Groups[1].Value},");
        text = Regex.Replace(text, @"(:[\d][\d\.]*)", m => $"{m.Groups[1].Value},");
        text = Regex.Replace(text, @":([\.][\d\.]*)", m => $":0{m.Groups[1].Value},");
        text = Regex.Replace(text, @"(:false|true)", m => $"{m.Groups[1].Value},");
        text = Regex.Replace(text, @",(\.\d)", m => $",0{m.Groups[1].Value},");
        text = Regex.Replace(text, @",([\r\n][\t\n]*)}", m => $"{m.Groups[1].Value}}}");
        text = Regex.Replace(text, @",([\r\n][\t\n]*)\]", m => $"{m.Groups[1].Value}]");
        text = Regex.Replace(text, @"\[(\.)", m => $"[0{m.Groups[1].Value}");

        // 3. 进一步处理key-value
        text = text.TrimEnd(',');
        // 4. 反序列化为对象再序列化为格式化JSON
        var obj = JObject.Parse(text);
        return obj;
        }

    /// <summary>
    /// 按顺序将 content 拆分为若干部分，每部分为以 "FE FF" 开头、"00 00" 结尾的片段（包含分隔符），其余为普通片段。
    /// </summary>
    /// <param name="content">要拆分的字节数组</param>
    /// <returns>拆分后的各部分（每部分为 byte[]）</returns>
    private static List<byte[]> SplitByUtf16BeBlocks(byte[] content)
        {
        var result = new List<byte[]>();
        var i = 0;
        while (i < content.Length)
            {
            // 查找下一个 FE FF
            if (i + 1 < content.Length && content[i] == 0xFE && content[i + 1] == 0xFF)
                {
                var start = i;
                i += 2;
                // 查找 00 00 结尾
                while (i + 1 < content.Length)
                    {
                    if (content[i] == 41 && content[i + 1] == 10)
                        {
                        i += 2;
                        break;
                        }

                    i += 2;
                    }

                var end = i;
                var block = new byte[end - start];
                Array.Copy(content, start, block, 0, block.Length);
                result.Add(block);
                }
            else
                {
                var start = i;
                // 查找下一个 FE FF
                while (
                    i < content.Length
                    && !(i + 1 < content.Length && content[i] == 0xFE && content[i + 1] == 0xFF)
                )
                    {
                    i++;
                    }

                if (i > start)
                    {
                    var block = new byte[i - start];
                    Array.Copy(content, start, block, 0, block.Length);
                    result.Add(block);
                    }
                }
            }

        return result;
        }
    }




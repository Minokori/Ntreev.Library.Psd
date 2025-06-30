using System.Collections;

namespace Ntreev.Library.Psd.Structures;

internal class StructureEngineData : Properties
    {
    public StructureEngineData(PsdBinaryReader reader)
        {
        var length = reader.ReadInt32();
        reader.Skip('\n', 2);
        ReadProperties(reader, 0, this);
        }

    private void ReadProperties(PsdBinaryReader reader, int level, Properties props)
        {
        reader.Skip('\t', level);
        var c = reader.ReadChar();
        if (c == ']')
            {
            return;
            }
        else if (c == '<')
            {
            reader.Skip('<');
            }

        reader.Skip('\n');
        while (true)
            {
            reader.Skip('\t', level);
            c = reader.ReadChar();
            if (c == '>')
                {
                reader.Skip('>');
                return;
                }
            else
                {
                c = reader.ReadChar();
                var name = string.Empty;
                while (true)
                    {
                    c = reader.ReadChar();
                    if (c is ' ' or (char)10)
                        {
                        break;
                        }

                    name += c;
                    }

                if (c == 10)
                    {
                    Properties p = [];
                    ReadProperties(reader, level + 1, p);
                    if (p.Count > 0)
                        props.Add(name, p);
                    reader.Skip('\n');
                    }
                else if (c == ' ')
                    {
                    var value = ReadValue(reader, level + 1);
                    if (value is float f)
                        {
                        props[name] = f;
                        }
                    else if (value is int i)
                        {
                        props[name] = i;
                        }
                    else if (value is string str)
                        {
                        props[name] = str;
                        }
                    else
                        {
                        }
                    }
                }
            }
        }

    private object ReadValue(PsdBinaryReader reader, int level)
        {
        var c = reader.ReadChar();
        if (c == ']') //93
            {
            return null;
            }
        else if (c == '(') //99
            {
            // unicode string
            var text = string.Empty;

            var stringSignature = reader.ReadInt16();
            stringSignature = (short)(stringSignature & 0xFFFF);
            //assert stringSignature == 0xFEFF;
            while (true)
                {
                var b1 = reader.ReadChar();
                if (b1 == ')') // 41
                    {
                    reader.Skip('\n'); //10
                    return text;
                    }

                var b2 = reader.ReadChar();
                if (b2 == '\\') //92
                    {
                    b2 = reader.ReadChar();
                    }

                if (b2 == 13) //'\r'
                    {
                    text += '\n';
                    }
                else
                    {
                    text += (char)((b1 << 8) | b2);
                    }
                }
            }
        else if (c == '[')
            {
            ArrayList list = [];
            // array
            c = reader.ReadChar();
            while (true)
                {
                if (c == ' ')
                    {
                    var val = ReadValue(reader, level);
                    if (val == null)
                        {
                        reader.Skip('\n');
                        return list;
                        }
                    else
                        {
                        list.Add(val);
                        }
                    }
                else if (c == 10)
                    {
                    Properties p = [];
                    this.ReadProperties(reader, level, p);
                    reader.Skip('\n');
                    if (p.Count == 0)
                        {
                        return list;
                        }
                    else
                        {
                        list.Add(p);
                        }
                    }
                else
                    {
                    //assert false;
                    }
                }
            }
        else
            {
            var value = string.Empty;
            do
                {
                value += c;
                c = reader.ReadChar();
                } while (c is not (char)10 and not ' ');

                {
                if (int.TryParse(value, out var f))
                    return f;
                }

                {
                if (float.TryParse(value, out var f))
                    return f;
                }

                {
                if (bool.TryParse(value, out var f))
                    return f;
                }

            return value;
            }
        }
    }

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
        //Properties properties = new Properties();
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
                //assert c == 9;
                c = reader.ReadChar();
                //assert c == '/' : "unknown char: " + c + " on level: " + level;
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
                    this.ReadProperties(reader, level + 1, p);
                    if (p.Count > 0)
                        props.Add(name, p);
                    reader.Skip('\n');
                    }
                else if (c == ' ')
                    {
                    var value = ReadValue(reader, level + 1);
                    //props.Add(name, value);
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
                        //assert false;
                        }
                    }
                }
            }
        }
    private object ReadValue(PsdBinaryReader reader, int level)
        {
        var c = reader.ReadChar();
        if (c == ']')
            {
            return null;
            }
        else if (c == '(')
            {
            // unicode string
            var text = string.Empty;

            var stringSignature = reader.ReadInt16() & 0xFFFF;

            //assert stringSignature == 0xFEFF;
            while (true)
                {
                var b1 = reader.ReadChar();
                if (b1 == ')')
                    {
                    reader.Skip('\n');
                    return text;
                    }

                var b2 = reader.ReadChar();
                if (b2 == '\\')
                    {
                    b2 = reader.ReadChar();
                    }

                if (b2 == 13)
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
                    var val = this.ReadValue(reader, level);
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
                }
            while (c is not (char)10 and not ' ');

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


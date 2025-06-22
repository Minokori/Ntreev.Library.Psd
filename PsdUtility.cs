//Released under the MIT License.
//
//Copyright (c) 2015 Ntreev Soft co., Ltd.
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//documentation files (the "Software"), to deal in the Software without restriction, including without limitation the 
//rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit 
//persons to whom the Software is furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the 
//Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE 
//WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
//OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.



namespace Ntreev.Library.Psd;

internal static class PsdUtility
    {
    public static byte[] DecodeRLE(byte[] source)
        {
        List<byte> dest = [];
        byte runLength;

        for (var i = 1; i < source.Length; i += 2)
            {
            runLength = source[i - 1];

            while (runLength > 0)
                {
                dest.Add(source[i]);
                runLength--;
                }
            }

        return dest.ToArray();
        }

    public static void DecodeRLE(byte[] src, byte[] dst, int packedLength, int unpackedLength)
        {
        var index = 0;
        var num2 = 0;
        var num3 = 0;
        byte num4 = 0;
        var num5 = unpackedLength;
        var num6 = packedLength;
        while ((num5 > 0) && (num6 > 0))
            {
            num3 = src[index++];
            num6--;
            if (num3 != 0x80)
                {
                if (num3 > 0x80)
                    {
                    num3 -= 0x100;
                    }

                if (num3 < 0)
                    {
                    num3 = 1 - num3;
                    if (num6 == 0)
                        {
                        throw new Exception("Input buffer exhausted in replicate");
                        }

                    if (num3 > num5)
                        {
                        throw new Exception(string.Format("Overrun in packbits replicate of {0} chars", num3 - num5));
                        }

                    num4 = src[index];
                    while (num3 > 0)
                        {
                        if (num5 == 0)
                            {
                            break;
                            }

                        dst[num2++] = num4;
                        num5--;
                        num3--;
                        }

                    if (num5 > 0)
                        {
                        index++;
                        num6--;
                        }

                    continue;
                    }

                num3++;
                while (num3 > 0)
                    {
                    if (num6 == 0)
                        {
                        throw new Exception("Input buffer exhausted in copy");
                        }

                    if (num5 == 0)
                        {
                        throw new Exception("Output buffer exhausted in copy");
                        }

                    dst[num2++] = src[index++];
                    num5--;
                    num6--;
                    num3--;
                    }
                }
            }

        if (num5 > 0)
            {
            for (num3 = 0; num3 < num6; num3++)
                {
                dst[num2++] = 0;
                }
            }
        }

    public static BlendMode ToBlendMode(string text)
        {
        return text.Trim() switch
            {
                "pass" => BlendMode.PassThrough,
                "norm" => BlendMode.Normal,
                "diss" => BlendMode.Dissolve,
                "dark" => BlendMode.Darken,
                "mul" => BlendMode.Multiply,
                "idiv" => BlendMode.ColorBurn,
                "lbrn" => BlendMode.LinearBurn,
                "dkCl" => BlendMode.DarkerColor,
                "lite" => BlendMode.Lighten,
                "scrn" => BlendMode.Screen,
                "div" => BlendMode.ColorDodge,
                "lddg" => BlendMode.LinearDodge,
                "lgCl" => BlendMode.LighterColor,
                "over" => BlendMode.Overlay,
                "sLit" => BlendMode.SoftLight,
                "hLit" => BlendMode.HardLight,
                "vLit" => BlendMode.VividLight,
                "lLit" => BlendMode.LinearLight,
                "pLit" => BlendMode.PinLight,
                "hMix" => BlendMode.HardMix,
                "diff" => BlendMode.Difference,
                "smud" => BlendMode.Exclusion,
                "fsub" => BlendMode.Subtract,
                "fdiv" => BlendMode.Divide,
                "hue" => BlendMode.Hue,
                "sat" => BlendMode.Saturation,
                "colr" => BlendMode.Color,
                "lum" => BlendMode.Luminosity,
                _ => BlendMode.Normal,
                };
        }

    public static UnitType ToUnitType(string text)
        {
        return text switch
            {
                "#Ang" => UnitType.Angle,
                "#Rsl" => UnitType.Density,
                "#Rlt" => UnitType.Distance,
                "#Nne" => UnitType.None,
                "#Prc" => UnitType.Percent,
                "#Pxl" => UnitType.Pixels,
                "#Pnt" => UnitType.Points,
                "#Mlm" => UnitType.Millimeters,
                _ => throw new NotSupportedException(),
                };
        }

    public static int DepthToPitch(int depth, int width)
        {
        return depth switch
            {
                1 => width,//NOT Sure
                8 => width,
                16 => width * 2,
                _ => throw new NotSupportedException(),
                };
        }
    }

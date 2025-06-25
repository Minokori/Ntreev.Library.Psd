namespace Ntreev.Library.Psd;

internal partial class Channel
    {
    private void PrivateReadData(
        PsdBinaryReader reader,
        int bps,
        CompressionType compressionType,
        int[] rlePackLengths
    )
        {
        var length = PsdUtility.DepthToPitch(bps, this.Width);
        this.Data = new byte[length * Height];
        switch (compressionType)
            {
            case CompressionType.Raw:
                reader.Read(this.Data, 0, this.Data.Length);
                break;

            case CompressionType.RLE:
                //逐行读取
                for (var i = 0; i < Height; i++)
                    {
                    var buffer = new byte[rlePackLengths[i]];
                    var dst = new byte[length];
                    reader.Read(buffer, 0, rlePackLengths[i]);
                    DecodeRLE(buffer, dst, rlePackLengths[i], length);

                    //解压至_data
                    // TODO this is the deep source
                    for (var j = 0; j < length; j++)
                        {
                        this.Data[(i * length) + j] = (byte)(dst[j] * this.Opacity);
                        }
                    }

                break;
            }
        }

    /// <summary>
    /// 解压RLE算法
    /// </summary>
    /// <param name="src">源</param>
    /// <param name="dst">目标</param>
    /// <param name="packedLength">源长度</param>
    /// <param name="unpackedLength">目标长度</param>
    /// <exception cref="Exception"></exception>
    private static void DecodeRLE(byte[] src, byte[] dst, int packedLength, int unpackedLength)
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
                        throw new Exception(
                            string.Format("Overrun in packbits replicate of {0} chars", num3 - num5)
                        );
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
    }

namespace Ntreev.Library.Psd;

internal partial class Channel
    {
    /// <summary>
    /// 初始化完整的通道数据 到 Data 里
    /// </summary>
    /// <param name="reader">私有Reader</param>
    /// <param name="depth"></param>
    /// <param name="compressionType"></param>
    /// <param name="rlePackLengths"></param>
    private void PrivateReadData(
        PsdBinaryReader reader,
        int depth,
        CompressionType compressionType,
        int[] rlePackLengths
    )
        {
        // 初始化 Data 的大小 (实际宽度 * 高度)
        var rowLength = PsdUtility.DepthToPitch(depth, Width);
        Data = new byte[rowLength * Height];


        // 如果是 RLE 压缩, 则需要读取每行的长度
        switch (compressionType)
            {
            case CompressionType.Raw:
                //直接将数据读入 Data
                _ = reader.Read(Data, 0, Data.Length);
                break;


            case CompressionType.RLE:
                //逐行读取
                for (var rowIndex = 0; rowIndex < Height; rowIndex++)
                    {
                    //读取该行压缩后的数据
                    var packedRowData = reader.ReadBytes(rlePackLengths[rowIndex]);

                    //解压
                    var rowData = DecodeRLE(packedRowData, rowLength);

                    //移动到 data 对应的位置
                    for (var j = 0; j < rowLength; j++)
                        {
                        Data[(rowIndex * rowLength) + j] = (byte)(rowData[j] * Opacity);

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
    private static byte[] DecodeRLE(byte[] src, int unpackedLength)
    //private static byte[] DecodeRLE(byte[] src, byte[] rowData, int packedLength, int unpackedLength)
        {
        var dst = new byte[unpackedLength];
        var index = 0;
        var num2 = 0;
        var num3 = 0;
        byte num4 = 0;
        var num5 = unpackedLength;
        var num6 = src.Length;
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

        return dst;
        }

    }

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

namespace Ntreev.Library.Psd.Readers;

internal class ImageDataSectionReader(PsdBinaryReader reader, FileHeaderSection fileHeaderSection)
    : ValueReader<Channel[]>(reader, true, fileHeaderSection)
    {
    private readonly ChannelType[] types =
    [
        ChannelType.Red,
        ChannelType.Green,
        ChannelType.Blue,
        ChannelType.Alpha,
    ];

    protected override long InitStreamLength() => GlobalReader.Length - GlobalReader.Position;

    protected override Channel[] ReadValue()
        {
        var fileHeader = (FileHeaderSection)UserData!;
        var channelCount = fileHeader.NumberOfChannels;
        var width = fileHeader.Width;
        var height = fileHeader.Height;
        var depth = fileHeader.Depth;
        var compressionType = reader.ReadAsCompressionType();

        var channels = new Channel[channelCount];

        for (var i = 0; i < channels.Length; i++)
            {
            var type = i < types.Length ? types[i] : ChannelType.Mask;
            channels[i] = new Channel(type, width, height, 0, depth)
                {
                CompressionType = compressionType,
                RlePackLengths =
                    compressionType == CompressionType.RLE
                        ? reader.ReadAsChannelRlePackLengths(height)
                        : [],
                };
            }

        for (var i = 0; i < channels.Length; i++)
            {
            channels[i].ReadImageStreamDirectly(reader);
            }

        // 处理透明度
        if (channels.Length == 4)
            {
            for (var i = 0; i < channels[3].Data.Length; i++)
                {
                var a = channels[3].Data[i] / 255.0f;

                for (var j = 0; j < 3; j++)
                    {
                    var r = channels[j].Data[i] / 255.0f;
                    var r1 = (a + r - 1f) * 1f / a;
                    channels[j].Data[i] = (byte)(r1 * 255.0f);
                    }
                }
            }

        return [.. channels.OrderBy(item => item.Type)];
        }
    }

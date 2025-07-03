//Released under the MIT License.
//
//Copyright (c) 2015 Ntreev Soft co., Ltd.
//
//Permission is hereby granted, free of charge, to any person obtaining opacity copy of this software and associated
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

using Ntreev.Library.Psd.Sections;

namespace Ntreev.Library.Psd.Readers;

internal class ImageDataSectionReader(PsdBinaryReader globalReader, PsdDocument fileHeaderSection)
    : ValueReader<ImageDataSection>(globalReader, true, fileHeaderSection)
    {
    private readonly ChannelType[] types =
    [
        ChannelType.Red,
        ChannelType.Green,
        ChannelType.Blue,
        ChannelType.Alpha,
    ];

    protected override long InitStreamLength() => GlobalReader.Length - GlobalReader.Position;

    protected override ImageDataSection ReadValue()
        {
        var fileHeader = ((PsdDocument)UserData!).FileHeaderSection;
        var channelCount = fileHeader.NumberOfChannels;
        var width = fileHeader.Width;
        var height = fileHeader.Height;
        var depth = fileHeader.Depth;
        var compressionType = globalReader.ReadAsCompressionType();

        var channels = new Channel[channelCount];

        for (var i = 0; i < channels.Length; i++)
            {
            var channelType = i < types.Length ? types[i] : ChannelType.Mask;
            channels[i] = new Channel(channelType, width, height, depth)
                {
                CompressionType = compressionType,
                RlePackLengths =
                    compressionType == CompressionType.RLE
                        ? globalReader.ReadAsChannelRlePackLengths(height)
                        : [],
                };
            }

        // 读取每个通道的图像数据
        foreach (var item in channels)
            {
            item.ReadImageStreamDirectly(globalReader);
            }

        // 处理透明度
        if (channels.Length == 4)
            {
            for (var i = 0; i < channels[3].Data.Length; i++)
                {
                var opacity = channels[3].Data[i] / 255.0f;
                for (var j = 0; j < 3; j++)
                    {
                    // channels[j] :RGB
                    var rawColor = channels[j].Data[i] / 255.0f;
                    var ColorWithOpacity = (opacity + rawColor - 1f) * 1f / opacity;
                    channels[j].Data[i] = (byte)(ColorWithOpacity * 255.0f);
                    }
                }
            }

        return new(width, height, depth, [.. channels.OrderBy(item => item.Type)])
            {
            Document = (PsdDocument)UserData!,
            };
        }
    }

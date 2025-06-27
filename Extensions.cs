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

public static class Extensions
    {
    public static byte[] MergeChannels(this IImageSource imageSource)
        {
        // 在这里访问 ChannelsReader.Value
        var channels = imageSource.Channels;
        if (channels.Length == 4)
            {
            var length = channels.Length;
            var num2 = channels[0].Data.Length;

            var buffer = new byte[imageSource.Width * imageSource.Height * length];
            var num3 = 0;
            for (var i = 0; i < num2; i++)
                {
                for (var j = channels.Length - 1; j >= 0; j--)
                    {
                    buffer[num3++] = channels[j].Data[i];
                    }
                }

            return buffer;
            }
        else //TODO channels.StreamLength == 3,now it's for psdfile
            {
            var channelNum = channels.Length; //通道数
            var singleChannelPixelNum = channels[0].Data.Length; //单通道图片的 byte 量

            var buffer = new byte[imageSource.Width * imageSource.Height * (channelNum + 1)];

            var pixelindex = 0; //像素指针
            for (var i = 0; i < singleChannelPixelNum; i++)
                {
                for (var j = channelNum - 1; j >= 0; j--)
                    {
                    buffer[pixelindex++] = channels[j].Data[i];
                    }

                //buffer[pixelindex++] = 255;
                buffer[pixelindex++] = buffer[(pixelindex - 3)..pixelindex].All(i => i == 255)
                    ? (byte)0
                    : (byte)255;
                }

            return buffer;
            }
        }

    public static IEnumerable<IPsdLayer> Descendants(this IPsdLayer layer) =>
        Descendants(layer, item => true);

    public static IEnumerable<IPsdLayer> Descendants(
        this IPsdLayer layer,
        Func<IPsdLayer, bool> filter
    )
        {
        foreach (var item in layer.Childs)
            {
            if (filter(item) == false)
                continue;

            yield return item;

            foreach (var child in item.Descendants(filter))
                {
                yield return child;
                }
            }
        }

    /// <summary>
    /// 递归遍历指定 <see cref="PsdLayer"/> 及其所有子层，返回包含自身及所有后代层的枚举序列。
    /// </summary>
    /// <param name="layer">要遍历的根 <see cref="PsdLayer"/> 实例。</param>
    /// <returns>包含自身及所有后代 <see cref="PsdLayer"/> 的 <see cref="IEnumerable{PsdLayer}"/> 序列。</returns>
    internal static IEnumerable<PsdLayer> Descendants(this PsdLayer layer)
        {
        yield return layer;

        foreach (var item in layer.Childs)
            {
            foreach (var child in item.Descendants())
                {
                yield return child;
                }
            }
        }
    }

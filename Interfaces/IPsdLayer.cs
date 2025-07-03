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

using Newtonsoft.Json.Linq;

namespace Ntreev.Library.Psd.Interfaces;

/// <summary>
/// Photoshop 图层接口，包括 <see cref="IChannel"/> 和其他相关属性"/>
/// </summary>
public interface IPsdLayer
    {
    /// <summary>
    /// 图层的蒙版模式
    /// </summary>
    BlendMode BlendMode { get; }

    /// <summary>
    /// 图层的子图层
    /// </summary>
    /// <remarks>
    /// 由于图层组会被视为一个虚拟图层，因此会有可能存在子图层的情况。
    /// </remarks>
    IPsdLayer[] Childs { get; }

    bool IsClipping { get; }

    /// <summary>
    /// 链接到的图层
    /// </summary>
    ILinkedLayer? LinkedLayer { get; }

    /// <summary>
    /// 图层名称
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 图层的父图层
    /// </summary>
    IPsdLayer Parent { get; }

    /// <summary>
    /// 图层的 属性
    /// </summary>
    JObject Resources { get; }

    /// <summary>
    /// 图层所属于的文档对象
    /// </summary>
    PsdDocument Document { get; }

    /// <summary>
    /// 图层的左边距
    /// </summary>
    int Left { get; }

    /// <summary>
    /// 图层的上边距
    /// </summary>
    int Top { get; }

    /// <summary>
    /// 图层的右边距
    /// </summary>
    int Right { get; }

    /// <summary>
    /// 图层的下边距
    /// </summary>
    int Bottom { get; }

    /// <summary>
    /// 图像宽度(像素
    /// </summary>
    int Width { get; }

    /// <summary>
    /// 图像高度(像素)
    /// </summary>
    int Height { get; }

    /// <summary>
    /// 数据位深度，通常为 8 或 16
    /// </summary>
    int Depth { get; }

    /// <summary>
    /// 图像的颜色通道
    /// </summary>
    IChannel[] Channels { get; }

    /// <summary>
    /// 图像的透明度
    /// </summary>
    float Opacity { get; }

    /// <summary>
    /// 是否有图像数据, 由 图层 和 文档 对象实现
    /// </summary>
    bool HasImage { get; }

    /// <summary>
    /// 是否有蒙版
    /// </summary>
    bool HasMask { get; }

    /// <summary>
    /// 由左往右,由上往下地 获得图层的所有像素数据
    /// </summary>
    /// <param name="whereToSetTransparent">一个委托, 接受一个 bgr 像素数组, 返回该像素值是否要被设置为透明</param>
    /// <returns></returns>
    /// <remarks>
    /// 像素数据的格式为 [Blue, Green, Red, Alpha].<para/>
    /// 若图层没有 Alpha 通道，将根据传入的委托将指定的颜色设置为透明 (默认为白色(255,255,255) )。
    /// </remarks>
    IEnumerable<byte[]> GetBgraPixels(Func<byte[], bool>? whereToSetTransparent = null)
        {
        var PixelData = Channels[^1]
            .Data.Zip(Channels[^2].Data, Channels[^3].Data)
            .Select(i => new[] { i.First, i.Second, i.Third });
        if (Channels.Length >= 4)
            { PixelData = Channels[^4].Data.Zip(PixelData, (a, rgb) => rgb.Append(a).ToArray()); }
        else
            {
            whereToSetTransparent ??= (bgr) => bgr.All(i => i == 255);
            PixelData = PixelData.Select(bgr => bgr.Append(whereToSetTransparent(bgr) ? (byte)0 : (byte)255).ToArray());
            }

        return PixelData;
        }
    }

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


/// <summary>
/// 图层蒙版数据 <para/>
/// 可以是 40 字节、24 字节或   4 字节（如果没有图层掩码）。
/// </summary>
internal class LayerMask : Properties
    {
    public int Top => this["Top"]!.ToObject<int>()!;
    public int Left => this["Left"]!.ToObject<int>()!;
    public int Bottom => this["Bottom"]!.ToObject<int>()!;
    public int Right => this["Right"]!.ToObject<int>()!;
    public byte Color => this["Color"]!.ToObject<byte>()!;

    public byte Flags => this["Flags"]!.ToObject<byte>()!;

    public int Width => Right - Left;

    public int Height => Bottom - Top;
    }

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

/// <summary>
/// 颜色模式数据部分的读取器<para/>
/// 只有索引颜色和双色调（请参阅文件头部分中的模式字段）具有颜色模式数据。对于所有其他模式，此部分只是 4 字节长度的字段，该字段设置为零
/// </summary>
/// <param name="reader"></param>
internal class ColorModeDataSectionReader(PsdReader reader) : LazyValueReader<byte[]>(reader, null)
    {
    protected override long InitStreamLength() => GlobalReader.ReadInt32();

    protected override byte[] ReadValue() =>
        StreamLength > 0 ? GlobalReader.ReadBytes((int)StreamLength) : [];
    }

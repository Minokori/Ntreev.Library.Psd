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

namespace Ntreev.Library.Psd.Readers.LayerAndMaskInformation;


/// <summary>
/// 提供 <see cref="ReadValue(PsdBinaryReader, object, out Ntreev.Library.Psd.Channel[])"/> 方法读取通道的数据
/// <para/>
/// 到 <paramref name="layer"/> 中
/// </summary>
/// <param name="reader"></param>
/// <param name="length"></param>
/// <param name="layer">Channels</param>
internal class ChannelsReader(PsdBinaryReader reader, long length, PsdLayer layer) : ValueReader<Channel[]>(reader, length, layer)
    {
    protected override Channel[] ReadValue()
        {
        var layer = UserData as PsdLayer;
        var records = layer.Records;

        using MemoryStream stream = new(GlobalReader.ReadBytes((int)StreamLength));
        using PsdBinaryReader r = new(stream) { Uri = GlobalReader.Uri, Version = GlobalReader.Version };
        ReadValue(r, layer.Depth, records.Channels);
        return records.Channels;
        }

    private static void ReadValue(PsdBinaryReader reader, int depth, Channel[] channels)
        {
        foreach (var item in channels)
            {
            var compressionType = reader.ReadCompressionType();
            item.ReadHeader(reader, compressionType);
            item.Read(reader, depth, compressionType);
            }
        }
    }


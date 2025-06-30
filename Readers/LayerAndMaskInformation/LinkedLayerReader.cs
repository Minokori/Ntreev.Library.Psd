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

internal class LinkedLayerReader(PsdBinaryReader reader) : ValueReader<LinkedLayer>(reader, true, null)
    {
    protected override long InitStreamLength() => (GlobalReader.ReadInt64() + 3) & (~3);

    protected override LinkedLayer ReadValue()
        {
        _ = GlobalReader.VerifySignatureIs("liFD");
        var version = GlobalReader.ReadInt32();
        var id = new Guid(GlobalReader.ReadAsPascalString());
        var name = GlobalReader.ReadString();
        var type = GlobalReader.ReadAsType();
        var creator = GlobalReader.ReadAsType();
        var length = GlobalReader.ReadInt64();
        var properties = GlobalReader.ReadBoolean() == true ? new DescriptorStructure(GlobalReader) : null;

        var isDocument = this.IsDocument(GlobalReader);
        LinkedDocumentReader documentReader = null;
        LinkedDocumnetFileHeaderReader fileHeaderReader = null;
        if (length > 0 && isDocument == true)
            {
            var position = GlobalReader.Position;
            documentReader = new LinkedDocumentReader(GlobalReader, length);
            GlobalReader.Position = position;
            fileHeaderReader = new LinkedDocumnetFileHeaderReader(GlobalReader, length);
            }

        return new LinkedLayer(name, id, documentReader, fileHeaderReader);
        }

    private bool IsDocument(PsdBinaryReader reader)
        {
        var position = reader.Position;
        try
            {
            return reader.ReadAsType() == "8BPS";
            }
        finally
            {
            reader.Position = position;
            }
        }
    }


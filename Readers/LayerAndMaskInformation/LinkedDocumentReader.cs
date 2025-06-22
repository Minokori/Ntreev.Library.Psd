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

internal class LinkedDocumentReader(PsdReader reader, long length)
    : LazyValueReader<PsdDocument>(reader, length, null)
    {
    protected override PsdDocument ReadValue()
        {
        if (IsDocument() == true)
            {
            using Stream stream = new RangeStream(
                GlobalReader.Stream,
                GlobalReader.Position,
                StreamLength
            );

            var document = PsdDocument.Create(stream, GlobalReader.Uri);
            //using PsdReader streamReader = new(stream, GlobalReader.Uri);
            //PsdDocument document = new InternalDocument() { BinaryReader = streamReader };
            document.InitSections();
            return document;
            }
        else
            {
            return null;
            }
        }

    private bool IsDocument()
        {
        var position = GlobalReader.Position;
        try
            {
            return GlobalReader.ReadAsType() == "8BPS";
            }
        finally
            {
            GlobalReader.Position = position;
            }
        }
    }

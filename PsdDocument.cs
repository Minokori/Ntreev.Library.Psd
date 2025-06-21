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

using Ntreev.Library.Psd.Readers;
using Ntreev.Library.Psd.Services;

namespace Ntreev.Library.Psd;

public partial class PsdDocument : IPsdLayer, IDisposable
    {
    private FileHeaderSectionReader fileHeaderSection;
    private ColorModeDataSectionReader colorModeDataSection;
    private ImageResourcesSectionReader imageResourcesSection;
    private LayerAndMaskInformationSectionReader layerAndMaskSection;
    private ImageDataSectionReader imageDataSection;
    private PsdReader reader;
    //private Uri baseUri;





    public void Dispose()
        {
        if (reader == null)
            return;

        this.reader.Dispose();
        this.reader = null;
        this.OnDisposed(EventArgs.Empty);
        }


    public event EventHandler Disposed;

    protected virtual void OnDisposed(EventArgs e) => Disposed?.Invoke(this, e);

    internal void Read(Stream stream, PsdUriResolver resolver, Uri uri)
        {
        reader = new PsdReader(stream) { Uri = uri };
        this.reader.ReadDocumentHeader();
        this.fileHeaderSection = new FileHeaderSectionReader(this.reader);
        this.colorModeDataSection = new ColorModeDataSectionReader(this.reader);
        this.imageResourcesSection = new ImageResourcesSectionReader(this.reader);
        this.layerAndMaskSection = new LayerAndMaskInformationSectionReader(this.reader, this);
        this.imageDataSection = new ImageDataSectionReader(this.reader, this);
        }
    }


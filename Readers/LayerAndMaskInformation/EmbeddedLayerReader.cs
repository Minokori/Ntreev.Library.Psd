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

internal class EmbeddedLayerReader(PsdBinaryReader reader) : ValueReader<EmbeddedLayer>(reader, true, null)
    {
    protected override long InitStreamLength() => (GlobalReader.ReadInt64() + 3) & (~3);

    private Uri ReadAbsoluteUri(Uri uri)
        {
        var props = new DescriptorStructure(reader);
        if (props.Contains("fullPath") == true)
            {
            //            var absoluteUri = new Uri(props["fullPath"] as string);
            var absoluteUri = new Uri(props["fullPath"].ToObject<string>());
            if (File.Exists(absoluteUri.LocalPath) == true)
                return absoluteUri;
            }

        if (props.Contains("relPath") == true)
            {
            //var relativePath = props["relPath"] as string;
            var relativePath = props["relPath"].ToObject<string>();
            var absoluteUri = PsdBinaryReader.Resolver.ResolveUri(uri, relativePath);
            if (File.Exists(absoluteUri.LocalPath) == true)
                return absoluteUri;
            }

        if (props.Contains("Nm") == true)
            {
            //var name = props["Nm"] as string;
            var name = props["Nm"].ToObject<string>();
            var absoluteUri = PsdBinaryReader.Resolver.ResolveUri(uri, name);
            if (File.Exists(absoluteUri.LocalPath) == true)
                return absoluteUri;
            }

        //return props.Contains("fullPath") == true ? new Uri(props["fullPath"] as string) : null;
        return props.Contains("fullPath") == true ? new Uri(props["fullPath"].ToObject<string>()) : null;

        }

    protected override EmbeddedLayer ReadValue()
        {
        GlobalReader.VerifySignatureIs("liFE");

        var version = GlobalReader.ReadInt32();

        var id = new Guid(GlobalReader.ReadAsPascalString(1));
        var name = GlobalReader.ReadString();
        var type = GlobalReader.ReadAsType();
        var creator = GlobalReader.ReadAsType();

        var length = GlobalReader.ReadInt64();
        Properties? properties = GlobalReader.ReadBoolean() == true ? new DescriptorStructure(GlobalReader) : null;
        var absoluteUri = ReadAbsoluteUri(GlobalReader.Uri);

        return new EmbeddedLayer(id, absoluteUri);
        }
    }


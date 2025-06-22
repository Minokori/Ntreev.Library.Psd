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
namespace Ntreev.Library.Psd.Services;


/// <summary>
/// <see cref="PsdDocument"/> 文档管理器接口, 支持通过绝对路径或文件名获取文档。
/// </summary>
public interface IDocumentManager
    {
    abstract PsdDocument GetDocument(Uri absoluteUri);
    abstract PsdDocument GetDocument(string filename);

    virtual Uri ResolveUri(Uri absoluteUri, string relativeUri)
        {
        // 绝对路径为空或不是绝对URI时，尝试将相对路径转换为绝对URI
        if (absoluteUri == null || (!absoluteUri.IsAbsoluteUri && absoluteUri.OriginalString.Length == 0))
            {
            Uri uri = new(relativeUri, UriKind.RelativeOrAbsolute);
            if (!uri.IsAbsoluteUri && uri.OriginalString.Length > 0)
                uri = new Uri(Path.GetFullPath(relativeUri));
            return uri;
            }

        return relativeUri == null || relativeUri.Length == 0
            ? absoluteUri
            : !absoluteUri.IsAbsoluteUri ? throw new NotSupportedException("PSD_RelativeUriNotSupported") : new Uri(absoluteUri, relativeUri);
        }


    }


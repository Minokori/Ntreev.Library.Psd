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

namespace Ntreev.Library.Psd.Interfaces;

/// <summary>
/// 链接图层接口
/// </summary>
public interface ILinkedLayer
    {
    /// <summary>
    /// 链接到的图层
    /// </summary>
    PsdDocument? Document
        {
        get;
        }


    /// <summary>
    /// 若链接图层是嵌入的其他 PSD, 其他图层的绝对路径, 通过 resolver 实现懒加载
    /// </summary>
    Uri? AbsoluteUri
        {
        get;
        }


    /// <summary>
    /// 是否是实际的 Psd 文档, 或只是Psd 文档内的链接图层
    /// </summary>
    bool HasDocument
        {
        get;
        }

    Guid ID
        {
        get;
        }

    string Name
        {
        get;
        }

    int Width
        {
        get;
        }

    int Height
        {
        get;
        }
    }

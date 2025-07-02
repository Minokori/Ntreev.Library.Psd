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
using Ntreev.Library.Psd.Interfaces;

namespace Ntreev.Library.Psd;

/// <summary>
/// 除了 EmbeddedLayer, 其他的链接图层实现
/// </summary>
/// <param name="name"></param>
/// <param name="id"></param>
/// <param name="documentReader"></param>
/// <param name="fileHeaderReader"></param>
internal class LinkedLayer(JObject info) : ILinkedLayer
    {
    public JObject Properties { get; init; } = info;

    public PsdDocument? Document => null;

    public Uri? AbsoluteUri => null;

    public bool HasDocument => false;

    public Guid ID => Properties.ToValue<Guid>("UniqueId");

    public string Name => Properties.ToValue<string>("OriginalFileName") ?? "";

    public int Width => -1;

    public int Height => -1;
    }

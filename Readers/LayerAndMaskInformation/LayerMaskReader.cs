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
using Ntreev.Library.Psd.Exceptions;

namespace Ntreev.Library.Psd.Readers.LayerAndMaskInformation;

internal class LayerMaskReader(PsdBinaryReader reader) : ValueReader<JObject>(reader, true, null)
    {
    protected override long InitStreamLength() => GlobalReader.ReadInt32();


    protected override JObject ReadValue()
        {
        switch (StreamLength)
            {
            case 4:
                return [];
            case 20:
                {
                return
                    new JObject()
                        {
                        ["Top"] = GlobalReader.ReadInt32(),
                        ["Left"] = GlobalReader.ReadInt32(),
                        ["Bottom"] = GlobalReader.ReadInt32(),
                        ["Right"] = GlobalReader.ReadInt32(),
                        };
                }

            ;
            case 40:
                {
                return new JObject()
                    {
                    ["Top"] = GlobalReader.ReadInt32(),
                    ["Left"] = GlobalReader.ReadInt32(),
                    ["Bottom"] = GlobalReader.ReadInt32(),
                    ["Right"] = GlobalReader.ReadInt32(),
                    ["Color"] = GlobalReader.ReadByte(),
                    ["Flags"] = GlobalReader.ReadByte(),
                    ["UserMaskDensity"] = GlobalReader.ReadByte(),
                    ["UserMaskFeather"] = GlobalReader.ReadDouble(),
                    ["VectorMaskDensity"] = GlobalReader.ReadByte(),
                    ["VectorMaskFeather"] = GlobalReader.ReadDouble(),
                    };
                }
            default:
                throw new InvalidFormatException(
                $"LayerMaskReader: Invalid stream length {StreamLength} for LayerMask. Expected 4, 20, or 40 bytes.");
            }
        }



    }

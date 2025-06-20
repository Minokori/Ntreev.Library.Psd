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

namespace Ntreev.Library.Psd
    {
    abstract partial class ValueReader<T>
        {
        private readonly PsdReader reader;
        private readonly int readerVersion;
        private readonly long position;
        private readonly long length;
        private readonly object? userData;
        private T value;
        private bool isRead;

        protected ValueReader(PsdReader reader, bool hasLength, object? userData)
            {
            if (hasLength == true)
                {
                this.length = this.OnLengthGet(reader);
                }

            this.reader = reader;
            this.readerVersion = reader.Version;
            this.position = reader.Position;
            this.userData = userData;

            if (hasLength == false)
                {
                this.Refresh();
                this.length = reader.Position - this.position;
                }
            else
                {
                //this.Refresh();
                }

            this.reader.Position = this.position + this.length;
            }

        protected ValueReader(PsdReader reader, long length, object? userData)
            {
            if (length < 0)
                throw new InvalidFormatException();
            this.reader = reader;
            this.length = length;
            this.readerVersion = reader.Version;
            this.position = reader.Position;
            this.userData = userData;

            if (this.length == 0)
                {
                this.Refresh();
                this.length = reader.Position - this.position;
                }
            else
                {
                //this.Refresh();
                }

            this.reader.Position = this.position + this.length;
            }

        private void Refresh()
            {
            //更新字节流的位置指针，从指定位置开始读取
            this.reader.Position = this.position;
            this.reader.Version = this.readerVersion;

            // 从reader提供的字节流中读取值到this.value中
            this.ReadValue(this.reader, this.userData, out this.value);

            // 更新字节流指针，便于继续读取
            if (this.length > 0)
                this.reader.Position = this.position + this.length;
            // 设置为已读取状态
            this.isRead = true;
            }


        protected virtual long OnLengthGet(PsdReader reader)
            {
            return reader.ReadLength();
            }


        protected abstract void ReadValue(PsdReader reader, object? userData, out T value);
        }
    }
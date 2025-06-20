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

using System.Text;

namespace Ntreev.Library.Psd
    {
    /// <summary>
    /// 提供对 <see cref="BinaryReader"/> 的封装，负责读取二进制字节流
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="resolver"></param>
    /// <param name="uri"></param>
    partial class PsdReader(Stream stream, PsdUriResolver resolver, Uri uri) : IDisposable
        {
        //private readonly BinaryReader _reader = new InternalBinaryReader(stream);
        private readonly BinaryReader _reader = new(stream);
        private readonly PsdUriResolver _resolver = resolver;
        private readonly Stream _stream = stream;
        private readonly Uri _uri = uri;
        private int _version = 1;

        #region Methods from BinaryReader.
        public string ReadAscii(int length)
            {
            return Encoding.ASCII.GetString(this._reader.ReadBytes(length));
            }
        /// <summary>
        /// see <seealso cref="BinaryReader.Read(byte[], int, int)"/>
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int Read(byte[] buffer, int index, int count)
            {
            return this._reader.Read(buffer, index, count);
            }
        public byte ReadByte()
            {
            return this._reader.ReadByte();
            }
        public byte[] ReadBytes(int count)
            {
            return this._reader.ReadBytes(count);
            }
        public bool ReadBoolean()
            {
            return ReverseValue(this._reader.ReadBoolean());
            }

        public double ReadDouble()
            {
            return ReverseValue(this._reader.ReadDouble());
            }

        public short ReadInt16()
            {
            return ReverseValue(this._reader.ReadInt16());
            }

        public int ReadInt32()
            {
            return ReverseValue(this._reader.ReadInt32());
            }

        public long ReadInt64()
            {
            return ReverseValue(this._reader.ReadInt64());
            }

        public ushort ReadUInt16()
            {
            return ReverseValue(this._reader.ReadUInt16());
            }

        public uint ReadUInt32()
            {
            return ReverseValue(this._reader.ReadUInt32());
            }

        public ulong ReadUInt64()
            {
            return ReverseValue(this._reader.ReadUInt64());
            }
        #endregion

        #region Validate Func. Void return but may throw a Exception.
        public void ValidateSignature(string signature)
            {
            string s = this.ReadType();
            if (s != signature)
                throw new InvalidFormatException();
            }

        public void ValidateSignature()
            {
            this.ValidateSignature(false);
            }

        public void ValidateSignature(bool check64bit)
            {
            if (this.VerifySignature(check64bit) == false)
                throw new InvalidFormatException();
            }

        private void ValidateDocumentSignature()
            {
            string signature = this.ReadType();

            if (signature != "8BPS")
                throw new InvalidFormatException();
            }

        public void ValidateInt16(short value, string name)
            {
            ValidateValue(value, name, () => this.ReadInt16());
            }

        public void ValidateInt32(int value, string name)
            {
            ValidateValue(value, name, () => this.ReadInt32());
            }

        public void ValidateType(string value, string name)
            {
            ValidateValue(value, name, () => this.ReadType());
            }
        #endregion


        public void Dispose()
            {
            this._reader.Close();
            }

        public string ReadType()
            {
            return this.ReadAscii(4);
            }
        public string ReadPascalString(int modLength)
            {
            byte count = this._reader.ReadByte();
            string text = string.Empty;
            if (count == 0)
                {
                Stream baseStream = this._reader.BaseStream;
                baseStream.Position += modLength - 1;
                return text;
                }
            byte[] bytes = this._reader.ReadBytes(count);
            text = Encoding.UTF8.GetString(bytes);
            for (int i = count + 1; (i % modLength) != 0; i++)
                {
                Stream stream2 = this._reader.BaseStream;
                stream2.Position += 1L;
                }
            return text;
            }

        public string ReadString()
            {
            int length = this.ReadInt32();
            if (length == 0)
                return string.Empty;

            byte[] bytes = this.ReadBytes(length * 2);
            for (int i = 0; i < length; i++)
                {
                int index = i * 2;
                (bytes[index + 1], bytes[index]) = (bytes[index], bytes[index + 1]);
                }

            if (bytes[^1] == 0 && bytes[^2] == 0)
                {
                length--;
                }

            var name = Encoding.Unicode.GetString(bytes, 0, length * 2);
            return name!;
            }

        public string ReadKey()
            {
            int length = this.ReadInt32();
            length = (length > 0) ? length : 4;
            return this.ReadAscii(length);
            }

        public char ReadChar()
            {
            return (char)this.ReadByte();
            }





        public double[] ReadDoubles(int count)
            {
            double[] values = new double[count];
            for (int i = 0; i < count; i++)
                {
                values[i] = this.ReadDouble();
                }
            return values;
            }


        public long ReadLength()
            {
            return this._version == 1 ? this.ReadInt32() : this.ReadInt64();
            }



        public void Skip(char c)
            {
            char ch = this.ReadChar();
            if (ch != c)
                throw new NotSupportedException();
            }

        public void Skip(char c, int count)
            {
            for (int i = 0; i < count; i++)
                {
                this.Skip(c);
                }
            }

        public ColorMode ReadColorMode()
            {
            return (ColorMode)this.ReadInt16();
            }

        public BlendMode ReadBlendMode()
            {
            return PsdUtility.ToBlendMode(this.ReadAscii(4));
            }

        public LayerFlags ReadLayerFlags()
            {
            return (LayerFlags)this.ReadByte();
            }

        public ChannelType ReadChannelType()
            {
            return (ChannelType)this.ReadInt16();
            }

        public CompressionType ReadCompressionType()
            {
            return (CompressionType)this.ReadInt16();
            }

        public void ReadDocumentHeader()
            {
            this.ValidateDocumentSignature();
            this.Version = this.ReadInt16();
            this.Skip(6);
            }




        }
    }

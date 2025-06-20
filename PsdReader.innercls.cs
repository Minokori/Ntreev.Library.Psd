using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntreev.Library.Psd
    {
    partial class PsdReader
        {
        class InternalBinaryReader(Stream stream) : BinaryReader(stream)
            {
            }
        }
    }

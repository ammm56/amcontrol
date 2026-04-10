using ProtocolLib.CommonLib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.S7Tcp.Core
{
    public class S7Message : INetMessage
    {
        public int ProtocolHeadBytesLength => 4;

        public byte[] HeadBytes { get; set; }

        public byte[] ContentBytes { get; set; }

        public bool CheckHeadBytesLegal(byte[] token)
        {
            if (HeadBytes == null) return false;

            if (HeadBytes[0] == 0x03 && HeadBytes[1] == 0x00)
                return true;
            else
                return false;
        }

        public int GetContentLengthByHeadBytes()
        {
            if (HeadBytes?.Length >= 4)
                return HeadBytes[2] * 256 + HeadBytes[3] - 4;
            else
                return 0;
        }

        public int GetHeadBytesIdentity() => 0;

        public byte[] SendBytes { get; set; }
    }
}

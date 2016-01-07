using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSTest
{
    internal static class HeadProtocol
    {
        internal static byte[] Get7BitEncodedInt(int value)
        {
            List<byte> bytes = new List<byte>(8);
            uint num = (uint)value;
            while (num >= 0x80)
            {
                bytes.Add((byte)(num | 0x80));
                num = num >> 7;
            }

            bytes.Add((byte)num);
            return bytes.ToArray();
        }

        internal static int Read7BitEncodedInt(byte[] buffer, int length, ref int offset)
        {
            byte num3;
            int num = 0;
            int num2 = 0;
            //offset = 0;

            if (buffer.Length <= offset)
            {
                throw new FormatException("Format_Bad7BitInt32");
            }

            do
            {
                if (num2 == 0x23)
                {
                    throw new FormatException("Format_Bad7BitInt32");
                }
                num3 = buffer[offset++];
                num |= (num3 & 0x7f) << num2;
                num2 += 7;
            }
            while ((num3 & 0x80) != 0 && length > offset);

            return num;

        }
    }
}

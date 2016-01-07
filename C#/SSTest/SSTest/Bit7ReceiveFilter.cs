using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSTest
{
    class Bit7ReceiveFilter : IReceiveFilter<Bit7RequestInfo>
    {
        public Bit7RequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            int byteCount = 0;
            try
            {
                byteCount = HeadProtocol.Read7BitEncodedInt(readBuffer, length, ref offset);
            }
            catch (Exception ex)
            {
                rest = 0;
                State = FilterState.Error;
                return null;
            }
            if (readBuffer.Length - offset < byteCount)
            {
                rest = 0;
                return null;
            }
            else
            {
                rest = 0;
                return null;
                //return new BinaryRequestInfo(byteCount.ToString(), null);
            }
        }

        public int LeftBufferSize
        {
            get;
            set;
        }

        public IReceiveFilter<Bit7RequestInfo> NextReceiveFilter
        {
            get;
            set;
        }

        public void Reset()
        {

        }

        public FilterState State
        {
            get;
            set;
        }
    }
}

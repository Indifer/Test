using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSTest
{
    public class Bit7RequestInfo : BinaryRequestInfo
    {
        public Bit7RequestInfo(string key, byte[] body)
            : base(key, body)
        {

        }
    }
}

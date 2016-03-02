using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsyncTcpServer
{
    /// <summary>
    /// TcpServerConfig
    /// <remarks>code by LiangYi</remarks>
    /// </summary>
    public class TcpServerConfig
    {
        public int AccreditReceiveTimeout { get; private set; }
        public int IPPoint { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="accreditReceiveTimeout"></param>
        /// <param name="receiveStringMax"></param>
        /// <param name="messageEnd"></param>
        /// <param name="ipPoint"></param>
        public TcpServerConfig(int accreditReceiveTimeout = 20 * 1000            
            , int ipPoint = 8181)
        {
            AccreditReceiveTimeout = accreditReceiveTimeout;
            IPPoint = ipPoint;
        }
    }
}

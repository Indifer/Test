using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsyncTcpClient
{
    /// <summary>
    /// TcpClientHostConfig
    /// <remarks>code by LiangYi</remarks>
    /// </summary>
    public class TcpClientConfig
    {
        public string ServerHostname { get; private set; }
        public int ServerIPPoint { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serverHostname"></param>
        /// <param name="serverIpPoint"></param>
        /// <param name="reConnNum"></param>
        /// <param name="maxReConnNum"></param>
        public TcpClientConfig(string serverHostname, int serverIpPoint)
        {
            ServerHostname = serverHostname;
            ServerIPPoint = serverIpPoint;
        }
    }
}

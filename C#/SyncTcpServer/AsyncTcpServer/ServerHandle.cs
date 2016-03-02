using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsyncTcpServer
{
    /// <summary>
    /// 授权
    /// <remarks>code by LiangYi</remarks>
    /// </summary>
    public interface ServerHandle
    {
        /// <summary>
        /// 授权
        /// </summary>
        /// <returns></returns>
        void Accredit(UserToken token, Action<UserToken, SessionHandle> accreditSession);
    }
}

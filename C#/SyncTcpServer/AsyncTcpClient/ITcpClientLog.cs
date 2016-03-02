using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsyncTcpClient
{
    /// <summary>
    /// 日志接口
    /// <remarks>code by LiangYi</remarks>
    /// </summary>
    public interface ITcpClientLog
    {
        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="message"></param>
        void LogInfo(string message, string fileName = null);
        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="message"></param>
        void LogError(string message, Exception ex, string fileName = null);
        /// <summary>
        /// 记录Debug信息
        /// </summary>
        /// <param name="message"></param>
        void LogDebug(string message, string fileName = null);
    }
}

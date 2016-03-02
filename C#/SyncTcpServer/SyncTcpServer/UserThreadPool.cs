using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SyncTcpServer
{
    /// <summary>
    /// 用户线程池
    /// </summary>
    internal static class UserThreadPool
    {
        /// <summary>
        /// 线程字典
        /// </summary>
        private static ConcurrentDictionary<string, Thread> Threads = new ConcurrentDictionary<string, Thread>();

        /// <summary>
        /// 添加用户token
        /// </summary>
        /// <param name="threadName"></param>
        /// <param name="thread"></param>
        public static Thread AddThread(string threadName, Thread thread)
        {
            thread = Threads.AddOrUpdate(threadName, thread, (key, val) =>
            {
                val.Abort();
                return Thread.CurrentThread;
            });
            return thread;
        }

        /// <summary>
        /// 获取线程对象
        /// </summary>
        /// <param name="threadName"></param>
        /// <returns></returns>
        public static Thread GetThread(string threadName)
        {
            Thread thread = null;
            Threads.TryGetValue(threadName, out thread);
            return thread;
        }

        /// <summary>
        /// 移除线程
        /// </summary>
        /// <param name="threadName"></param>
        /// <param name="thread"></param>
        /// <returns></returns>
        public static bool RemoveThread(string threadName, out Thread thread)
        {
            return Threads.TryRemove(threadName, out thread);
        }

        /// <summary>
        /// 获取线程数
        /// </summary>
        /// <returns></returns>
        public static int Count()
        {
            return Threads.Count();
        }

        /// <summary>
        /// 清除所有线程
        /// </summary>
        public static void Clear()
        {
            foreach (var t in Threads)
            {
                try
                {
                    t.Value.Abort();
                }
                catch { }
            }
            Threads.Clear();
        }
    }
}

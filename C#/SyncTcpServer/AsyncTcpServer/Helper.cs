using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

#if AsyncTcpClient
namespace AsyncTcpClient
#else
namespace AsyncTcpServer
#endif
{
    public static class Helper
    {
        public static void LogExceptions(this Task task, Action<Exception> logException)
        {
            task.ContinueWith(t =>
            {
                var aggException = t.Exception.Flatten();
                foreach (var exception in aggException.InnerExceptions)
                    logException(exception);
            },
            TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}

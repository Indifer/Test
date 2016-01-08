using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskConApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var tcs = new TaskCompletionSource<int>();
            
            var ct = new CancellationTokenSource(7000);
            ct.Token.Register(() =>
            {
                Console.WriteLine("Cancel");
                tcs.TrySetResult(1);
            }, useSynchronizationContext: false);

            Stopwatch sw = new Stopwatch();
            sw.Restart();
            Console.WriteLine("StartNew...");
            Task.Factory.StartNew(() =>
            {
                Task.Delay(5000).Wait();
                Console.WriteLine("StartNew...Over");
                tcs.TrySetResult(DateTime.Now.Second);

            });
            
            var i = tcs.Task.Result;
            sw.Stop();
            Console.WriteLine("over..." + sw.Elapsed.TotalSeconds);

            Console.ReadLine();
        }
    }
}

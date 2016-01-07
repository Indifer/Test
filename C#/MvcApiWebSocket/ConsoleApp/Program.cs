using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket4Net;

namespace ConsoleApp
{
    class Program
    {
        static int count = 10000;
        static int backCount = 0;
        static WebSocket ws;
        static DateTime dt;
        static void Main(string[] args)
        {
            backCount = 0;
            Test2();

            Test1();

            Console.ReadLine();

            backCount = 0;
            Test2();

            Test1();

            Console.ReadLine();

            backCount = 0;
            Test2();

            Test1();

            Console.ReadLine();

        }

        static void Test2()
        {
            dt = DateTime.Now;

            Parallel.For(0, count, (x) =>
            {
                HttpRequestHelper.GetResponse("http://192.168.2.23/home/Test", null);
            });
            Console.WriteLine((DateTime.Now - dt));
        }

        static void Test1()
        {

            ws = new WebSocket("ws://192.168.2.23/api/Default");
            ws.Error += ws_Error;
            ws.Closed += ws_Closed;
            ws.DataReceived += ws_DataReceived;
            ws.MessageReceived += ws_MessageReceived;
            ws.Opened += ws_Opened;
            ws.Open();
        }

        static void ws_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            System.Threading.Interlocked.Increment(ref backCount);
            if (backCount == count)
            {
                Console.WriteLine((DateTime.Now - dt));
            }
        }

        static void ws_Opened(object sender, EventArgs e)
        {
            dt = DateTime.Now;

            Parallel.For(0, count, (x) =>
            {
                ws.Send("hi");
            });
        }

        static void ws_DataReceived(object sender, DataReceivedEventArgs e)
        {
            ;
        }

        static void ws_Closed(object sender, EventArgs e)
        {
            ;
        }

        static void ws_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            ;
        }
    }
}

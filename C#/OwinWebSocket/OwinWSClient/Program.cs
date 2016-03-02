using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using System.Diagnostics;

namespace OwinWSClient
{
    class Program
    {
        static void Main(string[] args)
        {
            RunSample().Wait();
            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        public static async Task RunSample()
        {
            ClientWebSocket websocket = new ClientWebSocket();

            string url = "ws://localhost:7000/";
            Console.WriteLine("Connecting to: " + url);
            await websocket.ConnectAsync(new Uri(url), CancellationToken.None);

            int seed = 20000;
            Task[] taskList = new Task[seed];

            Stopwatch sw = new Stopwatch();
            sw.Restart();
            for (var i = 0; i < seed; i++)
            {
                //taskList[i] = Send(websocket);
                await Send(websocket);
            }

            //Task.WaitAll(taskList);
            sw.Stop();
            Console.WriteLine("seed:{0}, time:{1}", seed, sw.ElapsedMilliseconds);

            //if (result.CloseStatus.HasValue)
            //{
            //    Console.WriteLine("Closed; Status: " + result.CloseStatus + ", " + result.CloseStatusDescription);
            //}
            //else
            //{
            //    Console.WriteLine("Received message: " + Encoding.UTF8.GetString(incomingData, 0, result.Count));
            //}
        }

        public static async Task Send(ClientWebSocket websocket)
        {
            string message = "Hello World";
            //Console.WriteLine("Sending message: " + message);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await websocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);

            byte[] incomingData = new byte[1024];
            WebSocketReceiveResult result = await websocket.ReceiveAsync(new ArraySegment<byte>(incomingData), CancellationToken.None);
        }
    }
}

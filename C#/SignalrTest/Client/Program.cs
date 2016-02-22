using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static HubConnection hubConnection = null;
        static IHubProxy stockTickerHubProxy = null;
        static IHubProxy strongHubProxy = null;

        static void Main(string[] args)
        {
            hubConnection = new HubConnection("http://121.199.25.195:9005/");
            stockTickerHubProxy = hubConnection.CreateHubProxy("TickerHub");
            strongHubProxy = hubConnection.CreateHubProxy("StrongHub");
            //stockTickerHubProxy.On<string>("UpdateStockPrice", stock => Console.WriteLine("Stock update for {0} new price {1}", stock.Symbol, stock.Price));
            //await hubConnection.Start




            hubConnection.Start(new WebSocketTransport()).Wait();

            int seed = 10;
            //int iteration = 100;
            System.Threading.ThreadPool.SetMinThreads(50, 50);
            Task[] taskList = new Task[seed];

            Stopwatch sw = new Stopwatch();
            sw.Restart();
            for (var i = 0; i < seed; i++)
            {
                taskList[i] = Invoke();
            }

            Task.WaitAll(taskList);
            sw.Stop();

            Console.WriteLine("total second:{0}, count:{1}, tps:{2}", sw.ElapsedMilliseconds, seed, seed / sw.ElapsedMilliseconds * 1000);

            //strongHubProxy.On<string>("NewMessage", (x) =>
            //{
            //    Console.WriteLine(x);
            //});
            //strongHubProxy.Invoke("Send").Wait();

            Console.ReadLine();
        }


        public static Task Invoke()
        {
            return stockTickerHubProxy.Invoke<string>("NewContosoChatMessage", "abc", "efg");

            //Console.WriteLine(res);
            //res = stockTickerHubProxy.Invoke<string>("NewContosoChatMessage", "dga223", "a43gsdkh").Result;
            //Console.WriteLine(res);

            //var shops = stockTickerHubProxy.Invoke<IEnumerable<Shop>>("GetShops", new Shop() { Name = "ClientShop" }).Result;
            //foreach (var shop in shops)
            //{
            //    Console.WriteLine(shop.Name);
            //}
        }
    }
}

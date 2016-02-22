using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static long count = 0;
        static string host = System.Configuration.ConfigurationManager.AppSettings["host"];

        static void Main(string[] args)
        {
            using (Microsoft.Owin.Hosting.WebApp.Start<Startup>(host))
            {
                Console.WriteLine(host);
                Console.WriteLine("Press [enter] to quit...");
                Console.ReadLine();
            }
        }
    }




}

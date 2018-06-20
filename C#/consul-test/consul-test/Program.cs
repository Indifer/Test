using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace consul_test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Test().Wait();
            return;
            BuildWebHost(args).Run();
        }

        public static async Task Test()
        {
            using (var client = new ConsulClient(config =>
            {
                config.Address = new Uri("http://172.31.224.203:8500/");
            }))
            {
                while (true)
                {
                    var res = await client.Health.Service("getUsers", null, true, new QueryOptions() { WaitIndex = 210, WaitTime = new TimeSpan(0,0,10) });
                    Console.WriteLine(res.ToString());

                    await Task.Delay(1000);
                }

            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
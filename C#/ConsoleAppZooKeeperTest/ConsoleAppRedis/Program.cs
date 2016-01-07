using ServiceStack.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppRedis
{
    class Program
    {
        static void Main(string[] args)
        {
            var redisClient = new RedisClient("121.199.25.195", 6379);
            redisClient.AddItemToList("list", "11");

            var aa = redisClient.Lists["list"];
            Console.WriteLine(aa.Count);
            Console.WriteLine(aa[0]);

            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect("yhdcache0.redis.cache.windows.net,ssl=false,password=");
            
        }
    }
}

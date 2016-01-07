using CacheManager.Core;
using CacheManager.Redis;
using CacheManager.SystemRuntimeCaching;
using CacheManager.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheManagerConApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var cache = CacheFactory.Build("startCache", setting =>
            //{
            //    //setting.WithSystemRuntimeCacheHandle("startHandle");
            //    //setting.WithRedisCacheHandle("redis",);

            //    setting.WithUpdateMode(CacheUpdateMode.Up)
            //    .WithRedisConfiguration("redis", config =>
            //    {
            //        config.WithAllowAdmin().WithDatabase(0).WithEndpoint("127.0.0.1", 6379);
            //    })
            //    .WithRetryTimeout(100)
            //    .WithRedisCacheHandle("redis");


            //});

            var cache2 = CacheFactory.Build<User>("startCache2", setting =>
            {
                //setting.WithSystemRuntimeCacheHandle("startHandle");
                //setting.WithRedisCacheHandle("redis",);

                setting.WithUpdateMode(CacheUpdateMode.Up)
                .WithRedisConfiguration("redis", config =>
                {
                    config.WithAllowAdmin().WithDatabase(1).WithEndpoint("127.0.0.1", 6379);
                })
                .WithRetryTimeout(100)
                .WithRedisCacheHandle("redis");


            });

            //cache.Add("keyA", "valueA");
            //cache.Put("keyB", 23);
            //Console.WriteLine("KeyB is " + cache.Get("keyB"));      // should be 42
            //cache.Update("keyB", v => 42);
            //Console.WriteLine("KeyA is " + cache.Get("keyA"));      // should be valueA
            //Console.WriteLine("KeyB is " + cache.Get("keyB"));      // should be 42
            ////cache.Remove("keyA");
            //Console.WriteLine("KeyA removed? " + (cache.Get("keyA") == null).ToString());
            //Console.WriteLine("We are done...");
            //Console.ReadKey();


            cache2.Add("keyA", new User { ID = 1, Name = "gg" });
            Console.ReadKey();


        }

        public class User
        {
            public long ID { get; set; }
            public string Name { get; set; }
        }
    }
}

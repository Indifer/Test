using ShoppingMall.Mobile.WebSite.Core.APIClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Security.Cryptography.TripleDESCryptoServiceProvider tripleDes = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
            tripleDes.GenerateKey();
            Console.WriteLine("random tripleDes key:");
            StringBuilder sb1 = new StringBuilder(48);
            foreach (byte b in tripleDes.Key)
                sb1.Append(string.Format("{0:X2}", b));
            Console.WriteLine(sb1);

            int len = 128;
            byte[] buff = new byte[len / 2];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(buff);
            Console.WriteLine("random SHA1 key:");
            StringBuilder sb2 = new StringBuilder(len);
            for (int i = 0; i < buff.Length; i++)
                sb2.Append(string.Format("{0:X2}", buff[i]));
            Console.WriteLine(sb2);

            return;

            var data = new Dictionary<string, string>() { { "a", "33" }, { "b", ",444" } };
            int speed = 100;
            string res = null;

            Stopwatch sw = new Stopwatch();

            res = ClientAppAPIClient.GetInstance.Post("Default/Add", data);
            sw.Reset();
            sw.Restart();
            Console.WriteLine("start");
            for (var i = 0; i < speed; i++)
            {
                data["a"] = i.ToString();
                res = ClientAppAPIClient.GetInstance.Post("Default/Add", data);
                //Console.WriteLine(res);
            }
            Console.WriteLine("end");
            Console.WriteLine(sw.ElapsedMilliseconds);
            
            /**/

            //sw.Reset();
            //sw.Restart();
            //Console.WriteLine("start");
            //System.Threading.Tasks.Parallel.For(0, speed, (i) =>
            //{
            //    data["a"] = i.ToString();
            //    res = ClientAppAPIClient.GetInstance.Post("Default/Add", data);
            //    Console.WriteLine(res);

            //});
            //Console.WriteLine("end");
            //Console.WriteLine(sw.ElapsedMilliseconds);

            /**/

            res = ClientAppAPIClient2.GetInstance.Post("Default/Add", data);
            sw.Reset();
            sw.Restart();
            Console.WriteLine("start");
            for (var i = 0; i < speed; i++)
            {
                data["a"] = i.ToString();
                res = ClientAppAPIClient2.GetInstance.Post("Default/Add", data);
                //Console.WriteLine(res);
            }
            Console.WriteLine("end");
            Console.WriteLine(sw.ElapsedMilliseconds);

            /**/

            //sw.Reset();
            //sw.Restart();
            //Console.WriteLine("start");
            //System.Threading.Tasks.Parallel.For(0, speed, (i) =>
            //{
            //    data["a"] = i.ToString();
            //    res = ClientAppAPIClient2.GetInstance.Post("Default/Add", data);
            //    Console.WriteLine(res);

            //});
            //Console.WriteLine("end");
            //Console.WriteLine(sw.ElapsedMilliseconds);

            /**/

            //Console.WriteLine(res);

            Console.ReadLine();

        }
    }
}

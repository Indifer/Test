using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using SuperSocket.ProtoBase;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            int speed = 1;
            for (var i = 0; i < speed; i++)
            {
                CreateClient();
            }

            //System.Threading.Thread.Sleep(0);
            Console.ReadLine();
        }

        private static int _tcpHeartRate = 5 * 60 * 1000;
        public static void CreateClient()
        {
            var data = System.Text.Encoding.UTF8.GetBytes("Hellow\n");
            var length = data.Length;
            var hex = new byte[4];
            hex[0] = (byte)length;
            hex[1] = (byte)(length >> 8);
            hex[2] = (byte)(length >> 16);
            hex[3] = (byte)(length >> 24);
                        
            SuperSocket.ClientEngine.EasyClient<StringPackageInfo> client = new SuperSocket.ClientEngine.EasyClient<StringPackageInfo>();
            var port = new System.Net.IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            
            client.Connected += Client_Connected;

            client.Initialize(new MyReceiveFilter());
            client.ConnectAsync(port).Wait();

            while (true)
            {
                client.Send(new ArraySegment<byte>(hex));
                client.Send(new ArraySegment<byte>(data));

                Thread.Sleep(2000);
            }

            //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //socket.Connect("127.0.0.1", 12345);

            //if (socket.Connected)
            //{
            //    var count = socket.Send(
            //         new[]
            //             {
            //             new ArraySegment<byte>(hex),
            //            new ArraySegment<byte>(data)
            //             });

            //}

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Client_Connected(object sender, EventArgs e)
        {
            Console.WriteLine("Client_Connected");
        }
    }

    public class MyReceiveFilter : FixedHeaderReceiveFilter<StringPackageInfo>
    {
        public MyReceiveFilter() : base(4) { }

        public override StringPackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            var data = System.Text.Encoding.UTF8.GetString(bufferStream.Buffers);

            Console.WriteLine(data);
            return new StringPackageInfo("AAA", data, null);
        }

        protected override int GetBodyLengthFromHeader(IBufferStream bufferStream, int length)
        {
            var header = bufferStream.Buffers[0];
            int bodyLength = (int)(header.Array[header.Offset] | header.Array[header.Offset + 1] << 8 | header.Array[header.Offset + 2] << 16 | header.Array[header.Offset + 3] << 24);
            return bodyLength;
            //throw new NotImplementedException();
        }
    }




}
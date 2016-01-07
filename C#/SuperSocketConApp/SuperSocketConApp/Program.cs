using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.Common;

namespace SuperSocketConApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start the server!");

            Console.ReadKey();
            Console.WriteLine();

            var appServer = new MyServer();
            //appServer.NewSessionConnected += AppServer_NewSessionConnected;
            //appServer.NewRequestReceived += AppServer_NewRequestReceived;

            RootConfig rootConfig = new RootConfig();
            rootConfig.PerformanceDataCollectInterval = 60;
            rootConfig.DisablePerformanceDataCollector = false;
            ServerConfig config = new ServerConfig();
            config.Port = 12345;
            

            //Setup the appServer
            if (!appServer.Setup(rootConfig, config)) //Setup with listening port
            {
                Console.WriteLine("Failed to setup!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();

            //Try to start the appServer
            if (!appServer.Start())
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("The server started successfully, press key 'q' to stop it!");

            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            //Stop the appServer
            appServer.Stop();

            Console.WriteLine("The server was stopped!");
            Console.ReadKey();
        }

        //private static void AppServer_NewRequestReceived(MySession session, StringRequestInfo requestInfo)
        //{
        //    Console.WriteLine("AppServer_NewRequestReceived,session:{0},requestInfo:{1}", session.SessionID, requestInfo.Body);
        //    session.Send("Gdgasdhgashshd");
        //}

        //private static void AppServer_NewSessionConnected(MySession session)
        //{
        //    Console.WriteLine("AppServer_NewSessionConnected!");
        //}
    }

    //public class StringPackageInfo : SuperSocket.SocketBase.Protocol.RequestInfo<string>
    //{
    //    public StringPackageInfo(string key, string body) : base(key, body)
    //    { }
    //}

    public class MySession : AppSession<MySession>
    {
        public MySession() : base()
        {
        }

        protected override void OnSessionStarted()
        {
            base.OnSessionStarted();
        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            Console.WriteLine("OnSessionClosed");
            base.OnSessionClosed(reason);
        }
    }

    public class MyServer : AppServer<MySession>
    {
        public MyServer() : base(new DefaultReceiveFilterFactory<MyReceiveFilter, StringRequestInfo>())
        {
            base.SetupCommands(new Dictionary<string, ICommand<MySession, StringRequestInfo>>
            {
                { "testKey", new TestKey() }
            });
        }

        protected override void OnNewSessionConnected(MySession session)
        {
            Console.WriteLine("AppServer_NewSessionConnected!");
            base.OnNewSessionConnected(session);
        }

        protected override void OnSessionClosed(MySession session, CloseReason reason)
        {
            base.OnSessionClosed(session, reason);
        }
    }

    public class DoReceiveFilter : TerminatorReceiveFilter<SuperSocket.SocketBase.Protocol.StringRequestInfo>
    {
        private static byte[] Term = System.Text.Encoding.UTF8.GetBytes("$");

        public DoReceiveFilter()
            :base(Term)
        {

        }

        protected override StringRequestInfo ProcessMatchedRequest(byte[] data, int offset, int length)
        {
            string body = null;
            body = Encoding.UTF8.GetString(data, offset, length);
            var pack = new StringRequestInfo("a", body, null);

            Console.WriteLine(pack.Body);
            return pack;
        }
    }

    public class MyReceiveFilter : FixedHeaderReceiveFilter<SuperSocket.SocketBase.Protocol.StringRequestInfo>
    {
        public MyReceiveFilter()
            : base(4)
        {

        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            int bodyLength = (int)(header[offset] | header[offset + 1] << 8 | header[offset + 2] << 16 | header[offset + 3] << 24);
            return bodyLength;
        }

        protected override StringRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            string body = null;
            if (bodyBuffer != null)
            {
                body = Encoding.UTF8.GetString(bodyBuffer, offset, length);
            }
            var pack = new StringRequestInfo("testKey", body, null);

            Console.WriteLine(pack.Body);
            return pack;
        }

        //public override IReceiveFilter<StringRequestInfo> NextReceiveFilter
        //{
        //    get
        //    {
        //        return base.NextReceiveFilter;
        //    }
        //}

        //public override StringRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        //{
        //    return base.Filter(readBuffer, offset, length, toBeCopied, out rest);
        //}

        //protected override StringRequestInfo ProcessMatchedRequest(byte[] buffer, int offset, int length, bool toBeCopied)
        //{
        //    var body = Encoding.UTF8.GetString(buffer, offset, length);
        //    var pack = new StringRequestInfo(null, body, null);

        //    return pack;
        //}

        //protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        //{
        //    return System.BitConverter.ToInt32(header, 0);
        //}

        //protected override StringRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        //{
        //    var body = Encoding.UTF8.GetString(bodyBuffer, offset, length);
        //    var pack = new StringRequestInfo(null, body, null);

        //    return pack;
        //}
    }

    public class TestKey : StringCommandBase<MySession>
    {
        public override string Name
        {
            get
            {
                return "testKey";
            }
        }

        public override void ExecuteCommand(MySession session, StringRequestInfo requestInfo)
        {
            var data = Encoding.UTF8.GetBytes(requestInfo.Body + "enenen");
            var length = data.Length;
            var hex = new byte[4];
            hex[0] = (byte)length;
            hex[1] = (byte)(length >> 8);
            hex[2] = (byte)(length >> 16);
            hex[3] = (byte)(length >> 24);

            session.Send(new ArraySegment<byte>(hex));
            session.Send(new ArraySegment<byte>(data));

            ;
        }
    }


}

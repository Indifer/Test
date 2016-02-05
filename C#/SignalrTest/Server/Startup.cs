using Microsoft.AspNet.SignalR;
using Owin;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
#if DEBUG
            //appBuilder.UseErrorPage();
#endif
            appBuilder.MapSignalR();
        }
    }
    

    public class TickerHub : Hub
    {
        public string NewContosoChatMessage(string name, string message)
        {
            return name + message;
        }

        public List<Shop> GetShops(Shop shop)
        {
            List<Shop> list = new List<Shop>();
            list.Add(shop);
            shop.Name = "dddddddd";
            list.Add(shop);
            return list;
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
    }

    public class StrongHub : Hub<IClient>
    {
        public void Send(string message)
        {
            Clients.Caller.NewMessage("11");
        }
    }

    public interface IClient
    {
        void NewMessage(string message);
    }
}

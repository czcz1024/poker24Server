using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Poke24Server.Hubs
{
    public class TabHub : Hub
    {
        public override System.Threading.Tasks.Task OnConnected()
        {
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public void Ready()
        {
        }

        public void Say(string str)
        {
        }

        public void Pass()
        {

        }

        public void Poke(string hand)
        {
        }

    }
}
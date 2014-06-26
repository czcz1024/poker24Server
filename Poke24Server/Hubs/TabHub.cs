namespace Poke24Server.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNet.SignalR;

    using Poke24Server.Logic;
    using Poke24Server.Models;

    public class TabHub:Hub
    {
        public override System.Threading.Tasks.Task OnConnected()
        {
            var tab = this.Context.QueryString["tab"];
            var uid = this.Context.QueryString["uid"];
            Clients.Caller.test(Context.ConnectionId+" conn to "+tab);
            var t1=Groups.Add(Context.ConnectionId, tab);
            var t2=Groups.Add(Context.ConnectionId, tab + "_" + uid);
            Task.WaitAll(t1, t2);

            var tabid = Guid.Parse(tab);
            var uguid = Guid.Parse(uid);

            if (EnterTab(tabid, uguid))
            {
                RefreshInfo(tabid);
                RefreshUsers(tabid);
                RefreshYou(tabid, uguid);
            }
            else
            {
                Clients.Caller.err("full");
            }

            return base.OnConnected();
        }

        private void RefreshYou(Guid tabid, Guid uguid)
        {
            var tab = Tab.GetTab(tabid);
            
            var hand = tab.Users.FirstOrDefault(x => x.UserId == uguid);
            if (hand != null)
            {
                var t = tab.Info.WaitUser == uguid;
                var you = new { 
                    InHand=hand.InHand,
                    Ready=hand.IsOk,
                    Finish=hand.IsFinish,
                    Turn=t
                };
                Clients.Group(tabid.ToString() + "_" + uguid.ToString()).refreshYou(you);
            }
            
        }

        private void RefreshUsers(Guid tabid)
        {
            var tab = Tab.GetTab(tabid);
            var us = tab.Users.Select(x => new Seat { 
                IsFinish=x.IsFinish,
                IsOk=x.IsOk,
                UserId=x.UserId,
                UserName=x.UserName
            });
            Clients.Group(tabid.ToString()).refreshUsers(us);
        }

        private void RefreshInfo(Guid tabid)
        {
            var tab = Tab.GetTab(tabid);
            Clients.Group(tabid.ToString()).refreshInfo(tab.Info);
        }

        public override System.Threading.Tasks.Task OnReconnected()
        {
            Clients.Caller.test("re");
            return base.OnReconnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected()
        {
            var tab = this.Context.QueryString["tab"];
            var uid = this.Context.QueryString["uid"];
            Groups.Remove(Context.ConnectionId, tab);
            Groups.Remove(Context.ConnectionId, tab+"_"+uid);
            return base.OnDisconnected();

        }

        public bool EnterTab(Guid tabid, Guid uid)
        {
            var tab = Tab.GetTab(tabid);
            return tab.UserEnter(uid);
        }

        public void Ready(Guid tabid, Guid uid)
        {
            var tab = Tab.GetTab(tabid);
            var seat = tab.GetUser(uid);
            seat.IsOk = true;
            RefreshUsers(tabid);
            if (tab.Users.All(x => x.IsOk))
            {
                tab.Start();
                RefreshInfo(tabid);
            }
            RefreshYou(tabid, uid);
        }
    }
}
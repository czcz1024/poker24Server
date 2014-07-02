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
            //Clients.Caller.test(Context.ConnectionId+" conn to "+tab);
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
            //Clients.Caller.test("re");
            return base.OnReconnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected()
        {
            var tab = this.Context.QueryString["tab"];
            var uid = this.Context.QueryString["uid"];
            
            var tabid = Guid.Parse(tab);
            var uguid = Guid.Parse(uid);
            OutTab(tabid, uguid);

            return base.OnDisconnected();

        }

        public void OutTab(Guid tabid, Guid uid)
        {
            Groups.Remove(Context.ConnectionId, tabid.ToString());
            Groups.Remove(Context.ConnectionId, tabid + "_" + uid);
            var tabs = Tab.GetTab(tabid);
            tabs.UserOut(uid);

            RefreshInfo(tabid);
            RefreshUsers(tabid);

            Clients.Group(tabid.ToString()).test(uid + " out ");
        }

        public void Game(Guid guid)
        {
            var tab = Tab.GetTab(guid);
            //Clients.Caller.test(tab);
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

            var start = false;
            if (tab.Users.All(x => x.IsOk))
            {
                tab.Start();
                start = true;
                
            }
            RefreshInfo(tabid);
            RefreshUsers(tabid);
            if (start)
            {
                foreach (var item in tab.Users)
                {
                    RefreshYou(tabid, item.UserId);
                }
            }
            else
            {
                RefreshYou(tabid, tab.Info.WaitUser);
                RefreshYou(tabid,uid);
            }
        }

        public void Push(Guid tabid, Guid uid, IEnumerable<int> card, IEnumerable<int> real)
        {
            //string txt = uid + " has push cards to " + tabid + ":"+ card.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b) + "(" + real.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b) + ")";
            //Clients.Caller.test(txt);

            var tab = Tab.GetTab(tabid);
            tab.Push(uid,card, real);
            RefreshInfo(tabid);
            RefreshUsers(tabid);
            RefreshYou(tabid, uid);
            RefreshYou(tabid, tab.Info.WaitUser);
        }

        public void Pass(Guid tabid, Guid uid)
        {
            var tab = Tab.GetTab(tabid);
            tab.Pass(uid);
            RefreshInfo(tabid);
            RefreshUsers(tabid);
            RefreshYou(tabid, uid);
            RefreshYou(tabid, tab.Info.WaitUser);
        }
    }
}
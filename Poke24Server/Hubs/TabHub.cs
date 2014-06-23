namespace Poke24Server.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNet.SignalR;

    using Poke24Server.Models;

    public class TabHub:Hub
    {
        public override System.Threading.Tasks.Task OnConnected()
        {
            var tab = this.Context.QueryString["tab"];
            var uid = this.Context.QueryString["uid"];
            Groups.Add(Context.ConnectionId, tab);

            var id = Guid.Parse(tab);
            var userid = Guid.Parse(uid);
            var game = GameViewModel.GetGame(id);
            if (game != null)
            {
                game.Join(userid);

                game.SaveCache();
                Clients.Caller.test("join");
            }

            RefreshSeats(id);

            Clients.Caller.test(tab);
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnReconnected()
        {
            Clients.Caller.test("re");
            return base.OnReconnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected()
        {
            return base.OnDisconnected();

        }

        public void Ready(Guid id,Guid uid)
        {
            var game = GameViewModel.GetGame(id);
            var u = game.Seats.FirstOrDefault(x => x.UserId == uid && !x.IsOK);
            if (u != null)
            {
                u.IsOK = true;
                game.SaveCache();
                if (game.Seats.All(x => x.IsOK))
                {
                    BeginGame(id);
                    return;
                }
                RefreshSeats(id);
                Clients.Caller.test("is ready");
            }
            Clients.Caller.test("on ready");
        }

        public void Pass()
        {
        }

        public void Poke(string hand)
        {
        }

        public void BeginGame(Guid id)
        {
            Clients.Caller.test("begin");
        }

        public void RefreshSeats(Guid id)
        {
            var tab = id.ToString();
            var game = GameViewModel.GetGame(id);
            if (game == null)
            {
                Clients.Caller.test("no game");
            }
            else
            {
                Clients.Groups(new List<string> { tab }).refreshUsers(game.Seats);
                Clients.Group(tab).refreshUsers(game.Seats);
            }
        }
    }
}
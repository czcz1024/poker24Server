namespace Poke24Server.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Poke24Server.Models;

    public class AllTab
    {
        public static Dictionary<Guid, Tab> All { get; set; }

        static AllTab()
        {
            All = new Dictionary<Guid, Tab>();
        }

    }

    public class Tab
    {
        public TabInfo Info { get; set; }

        public List<Seat> Users { get; set; }

        public Tab(Guid id)
        {
            Info = new TabInfo();
            this.Info.Id = id;
            using (var db = new MockData())
            {
                var t = db.Tabs.FirstOrDefault(x => x.Id == id);
                Info.State = 0;
                if (t != null)
                {
                    this.Info.UserCnt = t.Player;
                    Users = new List<Seat>();
                    Info.LastHand = new List<Card>();
                    for (var i = 0; i < Info.UserCnt; i++)
                    {
                        Users.Add(new Seat { });
                    }
                }
                
            }
        }

        public static Tab GetTab(Guid id)
        {
            if (!AllTab.All.ContainsKey(id))
            {
                var obj = new Tab(id);
                AllTab.All.Add(id, obj);
            }
            return AllTab.All[id];
        }

        public bool UserEnter(Guid uid)
        {
            if (Users.Any(x => x.UserId == uid)) return true;
            if (Users.All(x => x.UserId != Guid.Empty))
            {
                return false;
            }

            if (Users.All(x => x.UserId == Guid.Empty))
            {
                Info.OwnerId = uid;
            }
            var seat = Users.FirstOrDefault(x => x.UserId == Guid.Empty);
            seat.UserId = uid;
            seat.UserName = new MockData().Users.FirstOrDefault(x => x.Id == uid).NickName;
            seat.IsOk = false;
            seat.IsFinish = false;
            return true;
        }
    }

    public class TabInfo
    {
        public Guid Id { get; set; }

        public int UserCnt { get; set; }

        public Guid OwnerId { get; set; }

        public Guid BigUser { get; set; }

        public Guid WaitUser { get; set; }

        public List<Card> LastHand { get; set; }

        public int State { get; set; }
    }

    public class Seat
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public bool IsOk { get; set; }

        public bool IsFinish { get; set; }

        public List<Card> InHand { get; set; }
    }

    public class Card
    {
        public string Text { get; set; }

        public int Value { get; set; }


    }
}
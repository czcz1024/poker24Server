using System.Collections.Concurrent;

namespace Poke24Server.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Poke24Server.Models;

    public class AllTab
    {
        public static ConcurrentDictionary<Guid, Tab> All { get; set; }

        static AllTab()
        {
            All = new ConcurrentDictionary<Guid, Tab>();
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
                AllTab.All.TryAdd(id, obj);
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

        public Seat GetUser(Guid uid)
        {
            var u = Users.FirstOrDefault(x => x.UserId == uid);
            if (u == null) throw new Exception("not have this user");
            return u;
        }

        public void Start()
        {
            initCard();
            this.Info.WaitUser = Info.OwnerId;
            Info.State = 1;
            
        }

        private void initCard()
        {
            var all = Card.SetOfCard();
            all=all.OrderBy(x=>Guid.NewGuid()).ToList();

            int i = 0;
            foreach (var item in all)
            {
                Users[i].InHand.Add(item);
                i++;
                if (i >= Users.Count) i = 0;
            }

            foreach (var u in Users)
            {
                u.InHand = u.InHand.OrderByDescending(x => x.Value).ToList();
            }
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

        public Seat()
        {
            InHand = new List<Card>();
        }
    }

    public class Card
    {
        public string Text { get; set; }

        public int Value { get; set; }

        public Card(int v)
        {
            this.Value = v;
            this.Text = map[v];
        }

        public static Dictionary<int, string> map = new Dictionary<int, string>();

        static Card ()
        {
            map.Add(1, "A");
            for (var i = 2; i <= 10; i++)
            {
                map.Add(i, i.ToString());
            }

            map.Add(11, "J");
            map.Add(12, "Q");
            map.Add(13, "K");
            map.Add(14, "A");
            map.Add(15, "2");

            map.Add(21, "XW");
            map.Add(22, "DW");
        }

        public static List<Card> SetOfCard()
        {
            var list = new List<Card>();
            for (var i = 0; i < 4; i++)
            {
                for (var j = 3; j <= 15; j++)
                {
                    list.Add(new Card(j));
                }
            }
            list.Add(new Card(21));
            list.Add(new Card(22));
            return list;
        }
    }
}
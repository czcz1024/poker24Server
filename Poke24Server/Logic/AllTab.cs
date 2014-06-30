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
            foreach (var item in new MockData().Tabs)
            {
                Tab.GetTab(item.Id);
            }
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
                        Users.Add(new Seat {Index=i });
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
            if (Info.State == 2)
            {
                using (var db = new MockData())
                {
                    var t = db.Tabs.FirstOrDefault(x => x.Id == Info.Id);
                    Info.State = 0;
                    Info.FinishCardPassSeat = null;
                    Info.LastRealHand = new List<Card>();
                    Info.LastBigHand = new List<Card>();
                    Info.BigUser = Guid.Empty;
                    Info.OwnerId = Guid.Empty;
                    Info.WaitUser = Guid.Empty;
                    
                    if (t != null)
                    {
                        Users = new List<Seat>();
                        Info.LastHand = new List<Card>();
                        for (var i = 0; i < Info.UserCnt; i++)
                        {
                            Users.Add(new Seat { Index = i });
                        }
                    }

                }
            }

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

        public void Pass(Guid uid)
        {
            SetNextUser(false);
            if (Info.FinishCardPassSeat != null)
            {
                Info.FinishCardPassSeat.Add(Info.WaitUser);

                if (Info.FinishCardPassSeat.Any(x => x == Info.WaitUser))
                {
                    //硬风
                    Info.LastBigHand = Info.LastRealHand;
                    Info.LastHand = new List<Card>();
                    Info.LastRealHand = new List<Card>();
                }
            }
        }

        public void Push(Guid uid, IEnumerable<int> card, IEnumerable<int> real)
        {
            var u = GetUser(uid);
            foreach (var item in real)
            {
                var c = u.InHand.FirstOrDefault(x => x.Value == item);
                if (c == null) throw new Exception("you don't have this card");
                u.InHand.Remove(c);
            }

            if (!u.InHand.Any())
            {
                SetUserFinish(uid);
            }
            if (Info.State == 2)
            {

                return;
            }
            var is44 = false;
            var realcard = real.Select(x => new Card(x)).ToList();
            var cardlist = card.Select(x => new Card(x)).ToList();
            if (IsPair(realcard) && realcard[0].Value == 4)
            {
                if (IsBoom(Info.LastHand))
                {
                    is44 = true;
                    Info.LastBigHand = realcard;
                    Info.LastHand = new List<Card>();
                    Info.LastRealHand = new List<Card>();

                }
                else
                {
                    Info.LastBigHand = new List<Card>();
                    Info.LastHand = cardlist;
                    Info.LastRealHand = realcard;
                }
            }
            else
            {
                Info.LastHand = cardlist;
                Info.LastRealHand = realcard;
            }
            
            Info.BigUser = uid;
            Info.FinishCardPassSeat = null;
            SetNextUser(is44);
        }

        private void SetNextUser(bool is44)
        {
            var nowpush =Users.FirstOrDefault(x=>x.UserId== Info.WaitUser);
            if (is44) return;

            var rest = Users.OrderBy(x => x.Index).FirstOrDefault(x => !x.IsFinish && x.Index > nowpush.Index);
            if (rest == null)
            {
                rest = Users.OrderBy(x => x.Index).FirstOrDefault(x => !x.IsFinish);
            }
            if (nowpush.UserId == rest.UserId)
            {
                Info.LastBigHand = Info.LastRealHand;
                Info.LastHand = new List<Card>();
                Info.LastRealHand = new List<Card>();
            }
            else
            {
                Info.LastBigHand = new List<Card>();
            }

            Info.WaitUser = rest.UserId;

            //todo check last big is self
            if (Info.WaitUser == Info.BigUser)
            {
                Info.LastBigHand = Info.LastRealHand;
                Info.LastHand = new List<Card>();
                Info.LastRealHand = new List<Card>();
            }
        }

        private void SetUserFinish(Guid uid)
        {
            //todo
            Info.FinishCardPassSeat = new List<Guid>();

            var u = GetUser(uid);
            u.IsFinish = true;
            var ufcnt = Users.Count(x => x.IsFinish);
            u.Rank = ufcnt;

            if (ufcnt == Info.UserCnt - 1)
            {
                var lastu = Users.FirstOrDefault(x => !x.IsFinish);
                lastu.IsFinish=true;
                lastu.Rank = Info.UserCnt;
                GameOver();
                
            }

        }

        public void GameOver()
        {
            Info.State = 2;
            
        }

        public bool IsBoom(List<Card> cards)
        {
            return cards.Count > 2 && IsAllSame(cards);
        }

        public bool IsAllSame(List<Card> cards)
        {
            return cards.All(x => x.Value == cards[0].Value);
        }

        public bool IsPair(List<Card> cards)
        {
            return cards.Count == 2 && IsAllSame(cards);
        }

        public void UserOut(Guid uguid)
        {
            var u=Users.FirstOrDefault(x => x.UserId == uguid);
            if (u != null)
            {
                Users.Remove(u);
                if (Info.State == 1)
                {
                    GameOver();
                }
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

        public List<Card> LastRealHand { get; set; }

        public List<Card> LastBigHand { get; set; }

        public int State { get; set; }

        public List<Guid> FinishCardPassSeat { get; set; }
    }

    public class Seat
    {
        public int Index { get; set; }

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public bool IsOk { get; set; }

        public bool IsFinish { get; set; }

        public List<Card> InHand { get; set; }

        public int Rank { get; set; }

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
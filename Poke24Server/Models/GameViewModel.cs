namespace Poke24Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Caching;

    using Poke24Server.Database;
    using System.Linq;
    public class GameViewModel
    {
        public Guid Id { get; set; }

        public List<Seat> Seats { get; set; }

        public GameViewModel(GameConfig config)
        {
            Seats = new List<Seat>();
            for (var i = 0; i < config.PlayerCount; i++)
            {
                Seats.Add(new Seat { });
            }
        }

        public static GameViewModel GetGame(Guid id)
        {
            var cache = MemoryCache.Default;
            var game = cache.Get("tab_" + id) as GameViewModel;
            if (game == null)
            {
                using (var db = new DataContext())
                {
                    var tab = db.Tabs.FirstOrDefault(x => x.Id == id);
                    if (tab != null)
                    {
                        var config = new GameConfig { 
                            PlayerCount=tab.Player,
                        };
                        game = new GameViewModel(config){
                            Id=id
                        };
                        cache.Set("tab_" + id, game, new CacheItemPolicy() { 
                            AbsoluteExpiration=new DateTimeOffset(DateTime.Now.AddDays(1))
                        });
                    }
                }
            }
            return game;
        }

        public bool Join(Guid userid)
        {
            if (!Seats.Any(x => x.UserId == userid))
            {
                var firstEmpty = Seats.FirstOrDefault(x => !x.HasUser);
                if (firstEmpty == null)
                {
                    return false;
                }
                else
                {
                    using (var db = new DataContext())
                    {
                        firstEmpty.HasUser = true;
                        firstEmpty.UserId = userid;
                        firstEmpty.UserName = db.Users.Find(userid).UserName;
                    }
                    return true;
                }
            }
            return false;
        }

        public void SaveCache()
        {
            var cache = MemoryCache.Default;
            cache.Set("tab_" + this.Id, this, new CacheItemPolicy()
            {
                AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddDays(1))
            });
        }
    }
}
namespace Poke24Server.Models
{
    using System.Collections.Generic;

    public class Card
    {
        public string Text { get; set; }

        public int Value { get; set; }

        public static List<Card> GetOne()
        {
            var list = new List<Card>();
            list.Add(new Card { Text="DW",Value=22});
            list.Add(new Card { Text="XW",Value=21});
            for (var i = 0; i < 4; i++)
            {
                list.Add(new Card { Text = "A",Value=15 });
                list.Add(new Card { Text = "2",Value=16 });
                for (var j = 3; j <= 10; j++)
                {
                    list.Add(new Card { Text = j.ToString(),Value=j });
                }
                list.Add(new Card { Text = "J",Value=11 });
                list.Add(new Card { Text = "Q",Value=12 });
                list.Add(new Card { Text = "K",Value=13 });
            }
            return list;
        }
    }
}
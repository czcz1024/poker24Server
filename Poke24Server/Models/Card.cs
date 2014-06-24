namespace Poke24Server.Models
{
    using System.Collections.Generic;
    using System.Linq;
    public class Card
    {
        private static Dictionary<int, string> ValueMapping = new Dictionary<int, string>();

        static Card()
        {
            ValueMapping.Add(22, "DW");
            ValueMapping.Add(21, "XW");


            ValueMapping.Add(15, "A");
            ValueMapping.Add(16, "2");
            for (var j = 3; j <= 10; j++)
            {
                ValueMapping.Add(j, j.ToString());
            }
            ValueMapping.Add(11, "J");
            ValueMapping.Add(12, "Q");
            ValueMapping.Add(13, "K");
            
        }

        public Card()
        {
        }

        public Card(int value)
        {
            this.Value = value;
            this.Text = ValueMapping[value];
        }

        public string Text { get; set; }

        public int Value { get; set; }

        public static List<Card> GetOne()
        {
            var list = new List<Card>();
            list.Add(new Card(22));
            list.Add(new Card(21));

            for (var i = 0; i < 4; i++)
            {
                list.Add(new Card(15));
                list.Add(new Card(16));
                for (var j = 3; j <= 13; j++)
                {
                    list.Add(new Card(j));
                }
            }
            return list;
        }
    }
}
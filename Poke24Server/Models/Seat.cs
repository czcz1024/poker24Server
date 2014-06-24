namespace Poke24Server.Models
{
    using System;
    using System.Collections.Generic;

    public class Seat
    {
        public bool HasUser { get; set; }

        public bool IsOK { get; set; }

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public bool MyTurn { get; set; }

        public List<Card> InHand { get; set; }

        public bool IsFinish { get; set; }

        public Seat()
        {
            InHand = new List<Card>();
        }
    }
}
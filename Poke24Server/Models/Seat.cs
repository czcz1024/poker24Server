namespace Poke24Server.Models
{
    using System;

    public class Seat
    {
        public bool HasUser { get; set; }

        public bool IsOK { get; set; }

        public Guid UserId { get; set; }

        public string UserName { get; set; }
    }
}
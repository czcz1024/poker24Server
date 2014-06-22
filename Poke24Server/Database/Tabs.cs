using System;

namespace Poke24Server.Database
{
    public class Tabs
    {
        public Guid Id { get; set; }
        public int Player { get; set; }

        public Guid Creator { get; set; }

        public bool InPlay { get; set; }
    }
}
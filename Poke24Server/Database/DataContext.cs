using System.Data.Entity;

namespace Poke24Server.Database
{
    public class DataContext:DbContext
    {
        public DbSet<Users> Users { get; set; }

        public DbSet<Tabs> Tabs { get; set; }
    }
}
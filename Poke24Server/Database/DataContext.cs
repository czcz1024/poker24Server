using System.Data.Entity;

namespace Poke24Server.Database
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class DataContext:DbContext
    {
        public DbSet<Users> Users { get; set; }

        public DbSet<Tabs> Tabs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasKey(x => x.Id);
            modelBuilder.Entity<Tabs>().HasKey(x => x.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
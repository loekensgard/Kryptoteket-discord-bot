using Kryptoteket.Bot.Models;
using Microsoft.EntityFrameworkCore;

namespace Kryptoteket.Bot.CosmosDB
{
    public class RegistryContext : DbContext
    {
        public DbSet<Reflink> Reflinks { get; set; }

        public RegistryContext(DbContextOptions options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reflink>()
                .HasKey(r => r.id);
        }
    }
}

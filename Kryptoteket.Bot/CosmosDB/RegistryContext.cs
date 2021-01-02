using Kryptoteket.Bot.Models;
using Microsoft.EntityFrameworkCore;

namespace Kryptoteket.Bot.CosmosDB
{
    public class RegistryContext : DbContext
    {
        public DbSet<Reflink> Reflinks { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<UserBet> UserBets { get; set; }

        public RegistryContext(DbContextOptions options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reflink>()
                .HasKey(r => r.id);

            modelBuilder.Entity<Bet>()
                .HasKey(b => b.id);

            modelBuilder.Entity<Bet>()
                .HasMany(b => b.Users)
                .WithOne()
                .HasForeignKey(u => u.BetId);

            modelBuilder.Entity<UserBet>()
                .HasKey(u => u.id);
        }
    }
}

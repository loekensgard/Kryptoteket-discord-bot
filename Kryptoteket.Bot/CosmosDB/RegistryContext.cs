using Kryptoteket.Bot.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Kryptoteket.Bot.CosmosDB
{
    public class RegistryContext : DbContext
    {
        public DbSet<RefUser> Reflinks { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<UserBet> UserBets { get; set; }
        public DbSet<BetWinner> BetWinners { get; set; }

        public DbSet<RefExchange> RefExchanges { get; set; }

        public RegistryContext(DbContextOptions options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefUser>()
                .HasKey(r => r.id);

            modelBuilder.Entity<RefExchange>()
                .HasKey(r => r.id);

            modelBuilder.Entity<Bet>()
                .HasKey(b => b.id);

            modelBuilder.Entity<Bet>()
                .HasMany(b => b.Users)
                .WithOne()
                .HasForeignKey(u => u.BetId);

            modelBuilder.Entity<UserBet>()
                .HasKey(u => u.id);

            modelBuilder.Entity<BetWinner>()
                .HasKey(r => r.id);

            modelBuilder.Entity<BetWinner>()
                .Property(b => b.BetsWon)
                .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
        }
    }
}

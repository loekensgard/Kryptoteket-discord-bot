using Kryptoteket.Bot.CosmosDB.Mappings;
using Kryptoteket.Bot.Models;
using Kryptoteket.Bot.Models.Bets;
using Kryptoteket.Bot.Models.Reflinks;
using Microsoft.EntityFrameworkCore;
using System;

namespace Kryptoteket.Bot.CosmosDB
{
    public class KryptoteketContext : DbContext
    {
        public DbSet<RefExchange> RefExchanges { get; set; }
        public DbSet<RefUser> RefUsers { get; set; }
        public DbSet<RefLink> RefLinks { get; set; }

        public DbSet<Bet> Bets { get; set; }
        public DbSet<BetUser> BetUsers { get; set; }
        public DbSet<PlacedBet> PlacedBets { get; set; }
        public DbSet<FinishedBetPlacement> FinishedBetPlacements { get; set; }

        public KryptoteketContext(DbContextOptions options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BetMap());
            modelBuilder.ApplyConfiguration(new BetUserMap());
            modelBuilder.ApplyConfiguration(new RefExchangeMap());
            modelBuilder.ApplyConfiguration(new RefLinkMap());
            modelBuilder.ApplyConfiguration(new RefUserMap());
            //modelBuilder.ApplyConfiguration(new RefExchangeRefUserMap());
            modelBuilder.ApplyConfiguration(new FinishedBetPlacementMap());
            modelBuilder.ApplyConfiguration(new PlacedBetMap());

        }
    }
}

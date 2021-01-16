using Kryptoteket.Bot.Models.Bets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kryptoteket.Bot.CosmosDB.Mappings
{
    class BetMap : IEntityTypeConfiguration<Bet>
    {
        public void Configure(EntityTypeBuilder<Bet> builder)
        {
            builder.HasKey(x => x.BetId);
            builder.HasMany(x => x.PlacedBets).WithOne().HasForeignKey(x => x.BetId);
        }
    }
}

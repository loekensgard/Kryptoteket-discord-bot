using Kryptoteket.Bot.Models.Bets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kryptoteket.Bot.CosmosDB.Mappings
{
    class BetUserMap : IEntityTypeConfiguration<BetUser>
    {
        public void Configure(EntityTypeBuilder<BetUser> builder)
        {
            builder.HasKey(x => x.BetUserId);
            builder.HasMany(x => x.Placements).WithOne().HasForeignKey(x => x.BetUserId);
        }
    }
}

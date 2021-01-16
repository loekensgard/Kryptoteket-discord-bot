using Kryptoteket.Bot.Models.Bets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Kryptoteket.Bot.CosmosDB.Mappings
{
    class FinishedBetPlacementMap : IEntityTypeConfiguration<FinishedBetPlacement>
    {
        public void Configure(EntityTypeBuilder<FinishedBetPlacement> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}

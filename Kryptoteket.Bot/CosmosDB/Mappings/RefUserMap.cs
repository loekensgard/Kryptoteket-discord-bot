using Kryptoteket.Bot.Models.Bets;
using Kryptoteket.Bot.Models.Reflinks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.CosmosDB.Mappings
{
    class RefUserMap : IEntityTypeConfiguration<RefUser>
    {
        public void Configure(EntityTypeBuilder<RefUser> builder)
        {
            builder.HasKey(x => x.RefUserId);
            builder.HasMany(x => x.Reflinks).WithOne().HasForeignKey(x => x.RefUserId);
        }
    }
}

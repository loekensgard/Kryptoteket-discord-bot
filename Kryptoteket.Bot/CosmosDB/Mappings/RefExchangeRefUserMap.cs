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
    class RefExchangeRefUserMap : IEntityTypeConfiguration<RefExchangeRefuser>
    {
        public void Configure(EntityTypeBuilder<RefExchangeRefuser> builder)
        {
            builder.HasKey(x => new { x.RefExchangeId, x.RefUserId });
            builder.HasOne(x => x.RefUser).WithMany().HasForeignKey(x => x.RefUserId);
            builder.HasOne(x => x.RefExchange).WithMany().HasForeignKey(x => x.RefExchangeId);
        }
    }
}

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
    class RefExchangeMap : IEntityTypeConfiguration<RefExchange>
    {
        public void Configure(EntityTypeBuilder<RefExchange> builder)
        {
            builder.HasKey(x => x.RefExchangeId);
            builder.HasMany(x => x.RefUsers).WithMany(x => x.RefExchanges).UsingEntity(x => x.ToTable("RefExchangeRefUsers"));
            builder.HasMany(x => x.Reflinks).WithOne().HasForeignKey(x => x.RefExchangeId);
        }
    }
}

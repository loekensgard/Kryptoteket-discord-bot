using Kryptoteket.Bot.Models.Reflinks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kryptoteket.Bot.CosmosDB.Mappings
{
    public class RefLinkMap : IEntityTypeConfiguration<RefLink>
    {
        public void Configure(EntityTypeBuilder<RefLink> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}

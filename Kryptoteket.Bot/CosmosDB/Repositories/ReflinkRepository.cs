using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.CosmosDB.Repositories
{
    public class ReflinkRepository : IReflinkRepository
    {
        private readonly RegistryContext _context;
        private readonly DbSet<Reflink> _set;

        public ReflinkRepository(RegistryContext context)
        {
            _context = context;
            _set = _context.Reflinks;

        }

        public async Task AddReflink(ulong id, string name, string reflink, ulong guildId)
        {
            _set.Add(new Reflink { id = id.ToString(), Name = name, Link = reflink, Approved = false, GuildId = guildId.ToString() });

            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(ulong id, ulong guildId)
        {
            var entity = await _set.AsQueryable().FirstOrDefaultAsync(r => r.id == id.ToString() && r.GuildId == guildId.ToString());
            return entity != null;
        }

        public async Task<List<Reflink>> GetReflinks(ulong guildId)
        {
            return await _set.AsAsyncEnumerable().Where(r => r.GuildId == guildId.ToString()).ToListAsync();
        }

        public async Task<Reflink> GetReflink(ulong id, ulong guildId)
        {
            return await _set.AsQueryable().FirstOrDefaultAsync(r => r.id == id.ToString() && r.GuildId == guildId.ToString());
        }

        public async Task UpdateReflink(ulong id, string reflink, ulong guildId)
        {
            var entity = await _set.AsQueryable().FirstOrDefaultAsync(r => r.id == id.ToString() && r.GuildId == guildId.ToString());
            if (entity != null)
            {
                if (entity.Link.ToLower() != reflink.ToLower()) entity.Link = reflink;
                _set.Update(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteReflink(ulong id, ulong guildId)
        {
            var entity = await _set.AsQueryable().FirstOrDefaultAsync(r => r.id == id.ToString() && r.GuildId == guildId.ToString());
            if (entity != null)
            {
                _set.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}

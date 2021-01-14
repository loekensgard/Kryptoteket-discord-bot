using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.CosmosDB.Repositories
{
    public class RefExchangeRepository : IRefExchangeRepository
    {
        private RegistryContext _context;
        private DbSet<RefExchange> _set;

        public RefExchangeRepository(RegistryContext context)
        {
            _context = context;
            _set = _context.RefExchanges;
        }

        public async Task CreateReflink(string refId, string exchange, string reflink, string userId)
        {
            _set.Add(new RefExchange { id = refId, Name = exchange, Link = reflink, UserId = userId });
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReflink(string id)
        {
            var entity = await _set.FindAsync(id);
            if (entity != null)
            {
                _set.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> Exists(string exchange)
        {
            var list = await _set.AsQueryable().ToListAsync();

            var entity = list.Find(x => x.Name.ToLower() == exchange.ToLower());
            return entity != null;
        }

        public async Task<RefExchange> GetRefExchange(string id)
        {
            return await _set.FindAsync(id);
        }

        public async Task<List<RefExchange>> GetRefExchanges(string userId = null, string exchange = null)
        {
            var query = _set.AsQueryable();

            if (userId != null) query = query.Where(x => x.UserId == userId);

            var cosmosSuck = await query.ToListAsync();
            if (exchange != null)
            {
                cosmosSuck.RemoveAll(x => x.Name.ToLower() != exchange.ToLower());
            }

            return cosmosSuck;
        }

        public async Task UpdateRefExchanges(string id, RefExchange reflink)
        {
            var entity = await _set.AsQueryable().FirstOrDefaultAsync(r => r.id == id);
            if (entity != null)
            {
                _context.Entry(entity).CurrentValues.SetValues(reflink);
                await _context.SaveChangesAsync();
            }
        }
    }
}

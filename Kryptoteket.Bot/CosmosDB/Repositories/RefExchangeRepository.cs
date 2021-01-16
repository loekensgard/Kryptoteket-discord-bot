using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models.Reflinks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.CosmosDB.Repositories
{
    public class RefExchangeRepository : IRefExchangeRepository
    {
        private KryptoteketContext _context;
        private DbSet<RefExchange> _set;

        public RefExchangeRepository(KryptoteketContext context)
        {
            _context = context;
            _set = _context.RefExchanges;
        }


        public async Task CreateRefExchange(RefExchange refExchange)
        {
            _set.Add(refExchange);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(string exchange)
        {
            var entity = await _set.AsQueryable().FirstOrDefaultAsync(x => x.Name.ToLower() == exchange.ToLower());
            return entity != null;
        }

        public async Task<RefExchange> GetRefExchange(string exchange)
        {
            return await _set.AsQueryable().Include(x => x.RefUsers).ThenInclude(x => x.Reflinks).FirstOrDefaultAsync(x => x.Name.ToLower() == exchange.ToLower());
        }

        public async Task<List<RefExchange>> GetRefExchanges(string exchange = null)
        {
            var query = _set.AsQueryable();

            if (!string.IsNullOrEmpty(exchange)) query = query.Where(x => x.Name.ToLower() == exchange.ToLower());

            return await query.Include(x => x.RefUsers).ThenInclude(x => x.Reflinks).ToListAsync();
        }

        public async Task<RefExchange> UpdateExchange(RefExchange refexch)
        {
            var entity = await _set.FindAsync(refexch.RefExchangeId);

            if(entity != null)
            {
                _context.Entry(entity).CurrentValues.SetValues(refexch);
                await _context.SaveChangesAsync();
            }

            return refexch;
        }
    }
}

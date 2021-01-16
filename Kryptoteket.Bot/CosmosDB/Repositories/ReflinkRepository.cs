using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models.Reflinks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.CosmosDB.Repositories
{
    public class ReflinkRepository : IReflinkRepository
    {
        private readonly KryptoteketContext _context;
        private readonly DbSet<RefLink> _set;

        public ReflinkRepository(KryptoteketContext context)
        {
            _context = context;
            _set = _context.RefLinks;
        }

        public async Task CreateReflink(RefLink refLink)
        {
                _set.Add(refLink);
                await _context.SaveChangesAsync();
        }

        public async Task DeleteReflink(int id)
        {
            var entity = await _set.FindAsync(id);

            if(entity != null)
            {
                _set.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> Exist(ulong? id = null, int? refExchangeId = null, string reflink = null)
        {
            var query = _set.AsQueryable();

            if (id.HasValue) query = query.Where(x => x.RefUserId == id);
            if (refExchangeId.HasValue) query = query.Where(x => x.RefExchangeId == refExchangeId);
            if (!string.IsNullOrEmpty(reflink)) query = query.Where(x => x.Link == reflink);

            var entity = await query.FirstOrDefaultAsync();
            return entity != null;
        }

        public async Task UpdateReflink(RefLink link)
        {
            var entity = await _set.FindAsync(link.Id);

            if (entity != null)
            {
                _context.Entry(entity).CurrentValues.SetValues(link);
                await _context.SaveChangesAsync();
            }
        }
    }
}

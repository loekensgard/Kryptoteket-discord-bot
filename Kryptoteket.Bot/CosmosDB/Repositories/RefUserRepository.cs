using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.CosmosDB.Repositories
{
    public class RefUserRepository : IRefUserRepository
    {
        private readonly RegistryContext _context;
        private readonly DbSet<RefUser> _set;

        public RefUserRepository(RegistryContext context)
        {
            _context = context;
            _set = _context.Reflinks;
        }

        public async Task CreateRefUser(string id, string name)
        {
            _set.Add(new RefUser { id = id, Name = name, Approved = false});
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(string id)
        {
            try {
                var entity = await _set.FindAsync(id);
                return entity != null;
            }
            catch
            {
                return false;
            }
        }

        public async Task<RefUser> GetRefUser(string id)
        {
            return await _set.AsQueryable().FirstOrDefaultAsync(r => r.id == id);
        }

        public async Task<List<RefUser>> GetRefUsers(string exchange = null)
        {
            return await _set.AsQueryable().ToListAsync();
        }

        public async Task Update(string id, RefUser reflink)
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

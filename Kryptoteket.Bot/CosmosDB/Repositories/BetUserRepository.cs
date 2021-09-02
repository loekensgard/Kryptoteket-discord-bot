using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models.Bets;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.CosmosDB.Repositories
{
    public class BetUserRepository : IBetUserRepository
    {
        private readonly KryptoteketContext _context;
        private readonly DbSet<BetUser> _set;

        public BetUserRepository(KryptoteketContext context)
        {
            _context = context;
            _set = _context.BetUsers;
        }

        public async Task AddBetUser(BetUser betUser)
        {
            _set.Add(betUser);
            await _context.SaveChangesAsync();
        }

        public async Task<BetUser> GetBetUser(ulong id)
        {
            return await _set.Include(x => x.Placements).FirstOrDefaultAsync(x => x.BetUserId == id);
        }

        public async Task<IEnumerable<BetUser>> GetUsers()
        {
            return await _set.ToListAsync();
        }
    }
}

using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models.Bets;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.CosmosDB.Repositories
{
    public class BetRepository : IBetRepository
    {
        private readonly KryptoteketContext _context;
        private readonly DbSet<Bet> _set;
        public BetRepository(KryptoteketContext context)
        {
            _context = context;
            _set = _context.Bets;
        }

        public async Task CreateBet(Bet bet)
        {
            _set.Add(bet);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBet(string shortName)
        {
            var entity = await _set.FindAsync(shortName);
            if (entity != null)
            {
                _set.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Bet> Getbet(string shortName)
        {
            return await _set.Include(x => x.PlacedBets).FirstOrDefaultAsync(x => x.ShortName == shortName);
        }
    }
}

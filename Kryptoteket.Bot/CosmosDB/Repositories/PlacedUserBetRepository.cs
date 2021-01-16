using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.CosmosDB.Repositories
{
    public class PlacedUserBetRepository : IPlacedUserBetRepository
    {
        private readonly KryptoteketContext _context;
        private readonly DbSet<PlacedBet> _set;
        public PlacedUserBetRepository(KryptoteketContext context)
        {
            _context = context;
            _set = _context.PlacedBets;
        }

        public async Task AddPlacedBet(PlacedBet userBet)
        {
            _set.Add(userBet);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> GetPlacedBet(int betId, ulong id)
        {
            var entity = await _set.AsQueryable().FirstOrDefaultAsync(x => x.BetId == betId && x.BetUserId == id);
            return entity != null; 
        }

        public async Task<List<PlacedBet>> GetPlacedBets(int betId)
        {
            return await _set.AsQueryable().Where(x => x.BetId == betId).ToListAsync();
        }
    }
}

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
            try
            {

                _set.Add(userBet);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var s = e;
            }

        }

        public async Task<bool> PlacedBetExists(int betId, ulong id)
        {
            var entity = await _set.AsQueryable().FirstOrDefaultAsync(x => x.BetId == betId && x.BetUserId == id);
            return entity != null; 
        }

        public async Task<List<PlacedBet>> GetPlacedBets(int betId)
        {
            return await _set.AsQueryable().Where(x => x.BetId == betId).ToListAsync();
        }

        public async Task<PlacedBet> GetPlacedBet(int betId, ulong id)
        {
            return await _set.AsQueryable().FirstOrDefaultAsync(x => x.BetId == betId && x.BetUserId == id);
        }

        public async Task UpdatePlacedBet(PlacedBet userBet)
        {
            var entity = await _set.FindAsync(userBet.Id);

            if(entity != null)
            {
                _context.Entry(entity).CurrentValues.SetValues(userBet);
                await _context.SaveChangesAsync();
            }
        }
    }
}

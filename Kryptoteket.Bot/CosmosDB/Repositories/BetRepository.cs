using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.CosmosDB.Repositories
{
    public class BetRepository : IBetRepository
    {
        private readonly RegistryContext _context;
        private readonly DbSet<Bet> _set;
        public BetRepository(RegistryContext context)
        {
            _context = context;
            _set = _context.Bets;
        }

        public async Task CreateBet(Bet bet)
        {
            try
            {
                _set.Add(bet);
                await _context.SaveChangesAsync();
            }
            catch(Exception)
            {
                throw;
            }
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
            return await _set.FirstAsync(b => b.id == shortName);
        }
    }
}

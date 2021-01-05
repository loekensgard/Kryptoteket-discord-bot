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
    public class BetWinnersRepository : IBetWinnersRepository
    {
        private readonly RegistryContext _context;
        private readonly DbSet<BetWinner> _set;
        public BetWinnersRepository(RegistryContext context)
        {
            _context = context;
            _set = _context.BetWinners;
        }

        public async Task AddWinner(BetWinner betWinner)
        {
            _set.Add(betWinner);
            await _context.SaveChangesAsync();
        }

        public async Task<BetWinner> GetBetWinner(string id)
        {
            return await _set.FindAsync(id);
        }
    }
}

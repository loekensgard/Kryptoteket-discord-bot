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
    public class UserBetRepository : IUserBetRepository
    {
        private readonly RegistryContext _context;
        private readonly DbSet<UserBet> _set;
        public UserBetRepository(RegistryContext context)
        {
            _context = context;
            _set = _context.UserBets;
        }

        public async Task AddUserBet(UserBet userBet)
        {
            _set.Add(userBet);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserBet>> GetUserBets(string id)
        {
            var list = _set.AsAsyncEnumerable().Where(r => r.BetId == id);

            return await list.ToListAsync();
        }
    }
}

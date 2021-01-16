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
        //private readonly KryptoteketContext _context;
        //private readonly DbSet<PlacedBet> _set;
        //public UserBetRepository(KryptoteketContext context)
        //{
        //    _context = context;
        //    _set = _context.UserBets;
        //}

        //public async Task AddUserBet(PlacedBet userBet)
        //{
        //    _set.Add(userBet);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task<List<PlacedBet>> GetUserBets(string id)
        //{
        //    var list = _set.AsAsyncEnumerable().Where(r => r.BetId == id);

        //    return await list.ToListAsync();
        //}
    }
}

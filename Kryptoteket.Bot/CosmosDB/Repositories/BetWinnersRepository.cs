using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.CosmosDB.Repositories
{
    public class BetWinnersRepository : IBetWinnersRepository
    {
        //private readonly KryptoteketContext _context;
        //private readonly DbSet<BetWinner> _set;
        //public BetWinnersRepository(KryptoteketContext context)
        //{
        //    _context = context;
        //    _set = _context.BetWinners;
        //}

        //public async Task AddWinner(BetWinner betWinner)
        //{
        //    _set.Add(betWinner);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task<BetWinner> GetBetWinner(string id)
        //{
        //    try
        //    {
        //        return await _set.FindAsync(id);
        //    }catch(Exception e)
        //    {
        //        Log.Error(e, e.Message);
        //        return null;
        //    }
        //}
    }
}

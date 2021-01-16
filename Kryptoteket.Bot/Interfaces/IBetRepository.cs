using Kryptoteket.Bot.Models;
using Kryptoteket.Bot.Models.Bets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface IBetRepository
    {
        Task CreateBet(Bet bet);
        Task DeleteBet(string shortName);
        Task<Bet> Getbet(string shortName);
    }
}

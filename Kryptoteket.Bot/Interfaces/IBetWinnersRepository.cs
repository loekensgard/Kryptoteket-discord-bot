using Kryptoteket.Bot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface IBetWinnersRepository
    {
        Task AddWinner(BetWinner betWinner);
        Task<BetWinner> GetBetWinner(string id);
    }
}

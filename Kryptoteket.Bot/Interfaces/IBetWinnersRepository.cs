using Kryptoteket.Bot.Models;
using Kryptoteket.Bot.Models.Bets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface IBetWinnersRepository
    {
        Task<List<FinishedBetPlacement>> GetBetWins(ulong userId);
    }
}

using Kryptoteket.Bot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface IPlacedUserBetRepository
    {
        Task<bool> PlacedBetExists(int betId, ulong id);

        Task<PlacedBet> GetPlacedBet(int betId, ulong id);

        Task AddPlacedBet(PlacedBet userBet);
        Task<List<PlacedBet>> GetPlacedBets(int betId);
        Task UpdatePlacedBet(PlacedBet userBet);
        Task<List<PlacedBet>> GetAllPlacedBets();
        Task UpdateName(ulong betUserId, string username);
    }
}

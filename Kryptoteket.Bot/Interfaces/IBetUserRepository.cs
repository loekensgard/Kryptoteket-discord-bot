using Kryptoteket.Bot.Models.Bets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface IBetUserRepository
    {
        Task<BetUser> GetBetUser(ulong id);
        Task AddBetUser(BetUser betUser);
        Task<IEnumerable<BetUser>> GetUsers();
        Task UpdateName(ulong betUserId, string username);
    }
}

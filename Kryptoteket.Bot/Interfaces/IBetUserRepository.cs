using Kryptoteket.Bot.Models.Bets;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface IBetUserRepository
    {
        Task<BetUser> GetBetUser(ulong id);
        Task AddBetUser(BetUser betUser);
    }
}

using Kryptoteket.Bot.Models;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface IMiraiexAPIService
    {
        Task<Ticker> GetTicker(string pair);
        Task<Price> GetPrice(string pair);
    }
}

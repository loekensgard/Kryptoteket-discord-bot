using Kryptoteket.Bot.Models;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface IMiraiexService
    {
        Task<Ticker> GetTicker(string pair);
        Task<Price> GetPrice(string pair);
    }
}

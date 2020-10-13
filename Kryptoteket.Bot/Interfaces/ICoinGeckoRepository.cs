using Kryptoteket.Bot.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface ICoinGeckoRepository
    {
        Task AddCurrency(List<CoinGeckoCurrency> coinGeckoCoins);
        Task<CoinGeckoCurrency> GetCurrency(string symbol);
    }
}

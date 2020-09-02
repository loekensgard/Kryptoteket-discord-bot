using Kryptoteket.Bot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface ICoinGeckoRepository
    {
        Task AddCurrency(List<CoinGeckoCurrency> coinGeckoCoins);
        Task<CoinGeckoCurrency> GetCurrency(string symbol);
    }
}

using Kryptoteket.Bot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface ICoinGeckoAPIService
    {
        Task<List<CoinGeckoCurrency>> GetCoinsList();
        Task<Price> GetPrice(string pair);
        Task<List<string>> GetSupportedVsCurrenciesList();
        Task<List<Gainers>> GetTopGainers(int top, string timePeriod);
    }
}

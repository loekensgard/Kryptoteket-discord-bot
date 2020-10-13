using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.InMemoryDB
{
    public class CoinGeckoRepository : ICoinGeckoRepository
    {
        public Dictionary<string, CoinGeckoCurrency> currencies = new Dictionary<string, CoinGeckoCurrency>();

        public async Task AddCurrency(List<CoinGeckoCurrency> coinGeckoCoins)
        {
            foreach (var coin in coinGeckoCoins)
            {
                currencies.TryAdd(coin.Symbol.ToLower(), coin);
            }
            Log.Information("Added {count} currencies", currencies.Keys.Count);

            await Task.CompletedTask;
        }

        public async Task<CoinGeckoCurrency> GetCurrency(string symbol)
        {
            currencies.TryGetValue(symbol.ToLower(), out var currency);
            return await Task.FromResult(currency);
        }
    }
}

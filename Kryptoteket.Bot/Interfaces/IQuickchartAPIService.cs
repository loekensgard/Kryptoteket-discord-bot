using Kryptoteket.Bot.Models;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface IQuickchartAPIService
    {
        Task<string> GetQuickchartURI(CoinGeckoCurrency coin, CoingGeckoSparkline sparkline);
    }
}

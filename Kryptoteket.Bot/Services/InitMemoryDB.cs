using Kryptoteket.Bot.Interfaces;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Services
{
    public class InitMemoryDB
    {
        private readonly ICoinGeckoAPIService _coinGeckoAPIService;
        private readonly ICoinGeckoRepository _coinGeckoRepository;

        public InitMemoryDB(ICoinGeckoAPIService coinGeckoAPIService, ICoinGeckoRepository coinGeckoRepository)
        {
            _coinGeckoAPIService = coinGeckoAPIService;
            _coinGeckoRepository = coinGeckoRepository;
        }

        public async Task InitDB()
        {
            var list = await _coinGeckoAPIService.GetCoinsList();
            await _coinGeckoRepository.AddCurrency(list);
        }
    }
}

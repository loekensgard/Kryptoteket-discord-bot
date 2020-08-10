using Discord;
using Discord.Commands;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Modules
{
    [Name("PriceCheckCommands")]
    public class PriceCheckCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IMiraiexAPIService _miraiexService;
        private readonly EmbedService _embedService;
        private readonly ICoinGeckoAPIService _coinGeckoAPI;

        public PriceCheckCommands(IMiraiexAPIService miraiexService, EmbedService embedService, ICoinGeckoAPIService coinGeckoAPI)
        {
            _miraiexService = miraiexService;
            _embedService = embedService;
            _coinGeckoAPI = coinGeckoAPI;
        }

        [Command("price", RunMode = RunMode.Async)]
        [Summary("Get price for pair from Miraiex")]
        public async Task GetPriceMiraiex(string pair)
        {
            var price = await _miraiexService.GetPrice(pair.Trim().ToLower());
            if (price == null){ await ReplyAsync($"The market {pair} is not supported", false); return; }

            var builder = _embedService.EmbedPrice(pair.Trim().ToUpper(), price, "MiraiEx");
            await ReplyAsync(null, false, builder.Build());
        }

        [Command("gainers", RunMode = RunMode.Async)]
        [Summary("Get gains of top coins")]
        public async Task GetTopCoinGainers(int top, string timePeriod = "24h")
        {
            if (top > 50){ await ReplyAsync("Top 50 is max"); return; }
            var topGainers = await _coinGeckoAPI.GetTopGainers(top, timePeriod.Trim().ToLower());
            
            var builder = _embedService.EmbedTopGainers(topGainers, top, timePeriod.Trim().ToLower());
            await ReplyAsync(null, false, builder.Build());
        }

        [Command("gainers", RunMode = RunMode.Async)]
        [Summary("Get top gains for top 100")]
        public async Task GetTopGainers(string timePeriod = "24h")
        {
            var topGainers = await _coinGeckoAPI.GetTopGainers(100,timePeriod.Trim().ToLower());

            var builder = _embedService.EmbedTop100Gainers(topGainers, timePeriod.Trim().ToLower());
            await ReplyAsync(null, false, builder.Build());
        }

        [Command("shitcoins", RunMode = RunMode.Async)]
        [Summary("Get top gains of shitcoins")]
        public async Task GetTopCoinGainers(string timePeriod = "24h")
        {
            var topGainers = await _coinGeckoAPI.GetTopShitcoins(timePeriod.Trim().ToLower());

            var builder = _embedService.EmbedTopShitcoins(topGainers, timePeriod.Trim().ToLower());
            await ReplyAsync(null, false, builder.Build());
        }


    }
}

using Discord.Commands;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using Kryptoteket.Bot.Services;
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
        public async Task GetPriceMiraiex(string pair, string exchange = null)
        {
            if (string.IsNullOrEmpty(pair)) { await ReplyAsync($"Pair cannot be empty", false); return; }

            var price = new Price();
            var source = "CoinGecko";

            if (!string.IsNullOrEmpty(exchange) && exchange.Trim().ToLower() == "mx")
            {
                source = "MiraiEx";
                price = await _miraiexService.GetPrice(pair.Trim().ToLower());
                if (price == null) { await ReplyAsync($"The market {pair} is not supported at {source}", false); return; }
            }
            else if (!string.IsNullOrEmpty(exchange) && exchange.Trim().ToLower() == "nbx")
            {
                await ReplyAsync($"Method not implemented", false); return;
            }
            else
            {
                price = await _coinGeckoAPI.GetPrice(pair.Trim().ToLower());
                if (price == null) { await ReplyAsync($"The market {pair} is not supported at {source}", false); return; }
            }

            var builder = _embedService.EmbedPrice(pair.Trim().ToUpper(), price, source);
            await ReplyAsync(null, false, builder.Build());
        }

        [Command("gainers", RunMode = RunMode.Async)]
        [Summary("Get gains of top coins")]
        public async Task GetTopCoinGainers(int top, string timePeriod = "24h")
        {
            if (top > 2000) { await ReplyAsync("Top 2000 is max"); return; }
            var topGainers = await _coinGeckoAPI.GetTopGainers(top, timePeriod.Trim().ToLower());

            var builder = _embedService.EmbedTopGainers(topGainers, top, timePeriod.Trim().ToLower());
            await ReplyAsync(null, false, builder.Build());
        }

    }
}

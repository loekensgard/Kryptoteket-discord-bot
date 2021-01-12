using Discord.Commands;
using Kryptoteket.Bot.Exceptions;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using Kryptoteket.Bot.Services;
using System;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Modules
{
    [Name("PriceCheckCommands")]
    public class PriceCheckCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IMiraiexAPIService _miraiexService;
        private readonly EmbedService _embedService;
        private readonly ICoinGeckoAPIService _coinGeckoAPI;
        private readonly INBXAPIService _nBXAPIService;

        public PriceCheckCommands(IMiraiexAPIService miraiexService, EmbedService embedService, ICoinGeckoAPIService coinGeckoAPI, INBXAPIService nBXAPIService)
        {
            _miraiexService = miraiexService;
            _embedService = embedService;
            _coinGeckoAPI = coinGeckoAPI;
            _nBXAPIService = nBXAPIService;
        }

        [Command("price", RunMode = RunMode.Async)]
        [Summary("Get price for pair from Miraiex")]
        public async Task GetPriceMiraiex(string pair, string exchange = null)
        {
            if (string.IsNullOrEmpty(pair)) { await ReplyAsync($"Pair cannot be empty", false); return; }

            var price = new Price();
            var source = "CoinGecko";
            var thumbnail = "";

            try
            {


                if (!string.IsNullOrEmpty(exchange) && exchange.Trim().ToLower() == "mx")
                {
                    source = "MiraiEx";
                    thumbnail = "https://res.cloudinary.com/climb/image/upload/v1582299567/ahaehiqfj7nqhhpu9cfz.png";
                    price = await _miraiexService.GetPrice(pair.Trim().ToLower());
                    if (price == null) { await ReplyAsync($"The market {pair} is not supported at {source}", false); return; }
                }
                else if (!string.IsNullOrEmpty(exchange) && exchange.Trim().ToLower() == "nbx")
                {
                    source = "NBX";
                    thumbnail = "https://pbs.twimg.com/profile_images/1090176246573600768/0fWX-0T3.jpg";
                    price = await _nBXAPIService.GetPrice(pair.Trim().ToLower());
                    if (price == null) { await ReplyAsync($"The market {pair} is not supported at {source}", false); return; }
                }
                else
                {
                    thumbnail = "https://static.coingecko.com/s/thumbnail-007177f3eca19695592f0b8b0eabbdae282b54154e1be912285c9034ea6cbaf2.png";
                    price = await _coinGeckoAPI.GetPrice(pair.Trim().ToLower());
                    if (price == null) { await ReplyAsync($"The market {pair} is not supported at {source}", false); return; }
                }
            }
            catch(ApiException e)
            {
                await ReplyAsync($"API failed with statuscode: {e.StatusCode}", false); return;
            }
            catch(NBXTradesNullException)
            {
                await ReplyAsync($"No trades last 24h", false); return;
            }
            catch (Exception e)
            {
                await ReplyAsync($"LOL: {e.Message}", false); return;
            }

            var builder = _embedService.EmbedPrice(pair.Trim().ToUpper(), price, source, thumbnail);
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

        [Command("losers", RunMode = RunMode.Async)]
        [Summary("Get losers of top coins")]
        public async Task GetTopCoinLosers(int top, string timePeriod = "24h")
        {
            if (top > 2000) { await ReplyAsync("Top 2000 is max"); return; }
            var topGainers = await _coinGeckoAPI.GetTopGainers(top, timePeriod.Trim().ToLower());

            var builder = _embedService.EmbedTopLosers(topGainers, top, timePeriod.Trim().ToLower());
            await ReplyAsync(null, false, builder.Build());
        }

    }
}

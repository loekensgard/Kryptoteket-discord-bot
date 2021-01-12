using Discord.Commands;
using Kryptoteket.Bot.Configurations;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using Kryptoteket.Bot.Services;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Modules
{
    [Name("TickerCheckCommands")]
    public class TickerCheckCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IMiraiexAPIService _miraiexService;
        private readonly EmbedService _embedService;
        private readonly INBXAPIService _nBXAPIService;
        private readonly ExchangesConfiguration _options;

        public TickerCheckCommands(IMiraiexAPIService miraiexService, EmbedService embedService, INBXAPIService nBXAPIService, IOptions<ExchangesConfiguration> options)
        {
            _miraiexService = miraiexService;
            _embedService = embedService;
            _nBXAPIService = nBXAPIService;
            _options = options.Value;
        }

        [Command("ticker", RunMode = RunMode.Async)]
        [Summary("Get ticker for pair from Miraiex and NBX")]
        public async Task GetTicker(string pair, string exchange)
        {
            if (string.IsNullOrEmpty(pair)) { await ReplyAsync($"Pair cannot be empty", false); return; }
            if (string.IsNullOrEmpty(exchange)) { await ReplyAsync($"Exchange cannot be empty", false); return; }

            var ticker = new Ticker();
            string source;
            string thumbnail;

            if(exchange.Trim().ToLower() == "mx")
            {
                source = "MiraiEx";
                thumbnail = _options.MiraiexIMG;
                ticker = await _miraiexService.GetTicker(pair.Trim().ToLower());
            }
            else if (exchange.Trim().ToLower() == "nbx")
            {
                source = "NBX";
                thumbnail = _options.NBXIMG;
                ticker = await _nBXAPIService.GetTicker(pair.Trim().ToLower());
            }
            else
            {
                await ReplyAsync($"WTF is {exchange}?", false); 
                return; 
            }

            if (ticker == null) { await ReplyAsync($"The market {pair} is not supported", false); return; }

            var builder = _embedService.EmbedTicker(pair.Trim().ToUpper(), ticker, source, thumbnail);
            await ReplyAsync(null, false, builder.Build());
        }
    }
}

using Discord.Commands;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Services;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Modules
{
    [Name("TickerCheckCommands")]
    public class TickerCheckCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IMiraiexService _miraiexService;
        private readonly EmbedService _embedService;

        public TickerCheckCommands(IMiraiexService miraiexService, EmbedService embedService)
        {
            _miraiexService = miraiexService;
            _embedService = embedService;
        }

        [Command("ticker btcnok", RunMode = RunMode.Async)]
        [Summary("Get btcnok ticker from Miraiex")]
        public async Task GetBTCTickerMiraiex()
        {
            var ticker = await _miraiexService.GetTicker("btcnok");
            var builder = _embedService.EmbedTicker("btcnok", ticker, "Miraiex");

            await ReplyAsync(null, false, builder.Build());
        }


        [Command("ticker ethnok", RunMode = RunMode.Async)]
        [Summary("Get ethnok ticker from Miraiex")]
        public async Task GetETHTickerMiraiex()
        {
            var ticker = await _miraiexService.GetTicker("ethnok");
            var builder = _embedService.EmbedTicker("ethnok", ticker, "Miraiex");

            await ReplyAsync(null, false, builder.Build());
        }

        [Command("ticker ltcnok", RunMode = RunMode.Async)]
        [Summary("Get btcnok ticker from Miraiex")]
        public async Task GetLTCTickerMiraiex()
        {
            var ticker = await _miraiexService.GetTicker("ltcnok");
            var builder = _embedService.EmbedTicker("ltcnok", ticker, "Miraiex");

            await ReplyAsync(null, false, builder.Build());
        }


        [Command("ticker xrpnok", RunMode = RunMode.Async)]
        [Summary("Get xrpnok ticker from Miraiex")]
        public async Task GetXRPTickerMiraiex()
        {
            var ticker = await _miraiexService.GetTicker("xrpnok");
            var builder = _embedService.EmbedTicker("xrpnok", ticker, "Miraiex");

            await ReplyAsync(null, false, builder.Build());
        }
    }
}

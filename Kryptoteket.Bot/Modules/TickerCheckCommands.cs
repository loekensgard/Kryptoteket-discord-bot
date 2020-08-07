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

        [Command("ticker", RunMode = RunMode.Async)]
        [Summary("Get ticker for pair from Miraiex")]
        public async Task GetTickerMiraiex([Remainder]string pair)
        {
            var ticker = await _miraiexService.GetTicker(pair.Trim().ToLower());
            if(ticker == null) await ReplyAsync($"The market {pair} is not supported", false);

            var builder = _embedService.EmbedTicker(pair.Trim().ToUpper(), ticker, "MiraiEx");
            await ReplyAsync(null, false, builder.Build());
        }
    }
}

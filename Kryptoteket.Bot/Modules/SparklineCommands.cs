using Discord.Commands;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Services;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Modules
{
    [Name("SparklineCommands")]
    public class SparklineCommands : ModuleBase<SocketCommandContext>
    {
        private readonly EmbedService _embedService;
        private readonly ICoinGeckoAPIService _coinGeckoAPIService;

        public SparklineCommands(EmbedService embedService, ICoinGeckoAPIService coinGeckoAPIService)
        {
            _embedService = embedService;
            _coinGeckoAPIService = coinGeckoAPIService;
        }

        [Command("graph", RunMode = RunMode.Async)]
        [Summary("Get graph")]
        public async Task GetHelpText(string currency)
        {
            var uri = await _coinGeckoAPIService.Get7dChart(currency);
            if(uri == null) { await ReplyAsync($"Could not find {currency}"); return; }

            await ReplyAsync(null, false, _embedService.EmbedSparkline(uri).Build());
        }
    }
}

using Discord.Commands;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Services;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Modules
{
    [Name("PriceCheckCommands")]
    public class PriceCheckCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IMiraiexService _miraiexService;
        private readonly EmbedService _embedService;

        public PriceCheckCommands(IMiraiexService miraiexService, EmbedService embedService)
        {
            _miraiexService = miraiexService;
            _embedService = embedService;
        }

        [Command("price btcnok", RunMode = RunMode.Async)]
        [Summary("Get btcnok price from Miraiex")]
        public async Task GetBTCPriceMiraiex()
        {
            var price = await _miraiexService.GetPrice("btcnok");
            var builder = _embedService.EmbedPrice("btcnok", price);

            await ReplyAsync(null, false, builder.Build());
        }

        [Command("price ethnok", RunMode = RunMode.Async)]
        [Summary("Get ethnok price from Miraiex")]
        public async Task GetETHPriceMiraiex()
        {
            var price = await _miraiexService.GetPrice("ethnok");
            var builder = _embedService.EmbedPrice("ethnok", price);

            await ReplyAsync(null, false, builder.Build());
        }

        [Command("price ltcnok", RunMode = RunMode.Async)]
        [Summary("Get ltcnok price from Miraiex")]
        public async Task GetLTCPriceMiraiex()
        {
            var price = await _miraiexService.GetPrice("ltcnok");
            var builder = _embedService.EmbedPrice("ltcnok", price);

            await ReplyAsync(null, false, builder.Build());
        }

        [Command("price xrpnok", RunMode = RunMode.Async)]
        [Summary("Get xrpnok price from Miraiex")]
        public async Task GetXRPPriceMiraiex()
        {
            var price = await _miraiexService.GetPrice("xrpnok");
            var builder = _embedService.EmbedPrice("xrpnok", price);

            await ReplyAsync(null, false, builder.Build());
        }
    }
}

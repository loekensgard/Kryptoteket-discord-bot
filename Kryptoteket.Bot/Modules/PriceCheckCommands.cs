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

        [Command("price", RunMode = RunMode.Async)]
        [Summary("Get price for pair from Miraiex")]
        public async Task GetPriceMiraiex([Remainder]string pair)
        {
            var price = await _miraiexService.GetPrice(pair.Trim().ToLower());
            if (price == null) await ReplyAsync($"The market {pair} is not supported", false);

            var builder = _embedService.EmbedPrice(pair.Trim().ToUpper(), price, "Miraiex");
            await ReplyAsync(null, false, builder.Build());
        }
    }
}

using Discord.Commands;
using Kryptoteket.Bot.Services;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Modules
{
    [Name("DefaultCommands")]
    public class DefaultCommands : ModuleBase<SocketCommandContext>
    {
        private readonly EmbedService _embedService;

        public DefaultCommands(EmbedService embedService)
        {
            _embedService = embedService;
        }

        [Command("help", RunMode = RunMode.Async)]
        [Alias("wtf", "info")]
        [Summary("Get help")]
        public async Task GetHelpText()
        {
            await ReplyAsync(null, false, _embedService.Embedhelp().Build());
        }

        [Command("support", RunMode = RunMode.Async)]
        [Summary("Support the developer")]
        public async Task GetSupportText()
        {
            await ReplyAsync(null, false, _embedService.EmbedSupportMe().Build());
        }


    }
}

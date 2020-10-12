using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Services;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

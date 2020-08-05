using Discord.Commands;
using Kryptoteket.Bot.Services;
using System;
using System.Collections.Generic;
using System.Text;
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


    }
}

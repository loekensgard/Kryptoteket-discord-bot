using Discord.Commands;
using Discord.WebSocket;
using Kryptoteket.Bot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Modules
{
    [Name("StatsCommands")]
    public class ServerCommands : ModuleBase<SocketCommandContext>
    {
        private readonly EmbedService _embedService;

        public ServerCommands(EmbedService embedService)
        {
            _embedService = embedService;
        }

        [Command("serverinfo", RunMode = RunMode.Async)]
        [Summary("Get serverinfo")]
        public async Task GetServerInfo()
        {
            var guild = Context.Guild;
            await guild.DownloadUsersAsync();

            await ReplyAsync(null, false, _embedService.EmbedServerInfo(guild).Build());
        }
    }
}

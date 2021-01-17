using Discord.Commands;
using Discord.WebSocket;
using Kryptoteket.Bot.Interfaces;
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
        private readonly IBetUserRepository _betUserRepository;

        public ServerCommands(EmbedService embedService, IBetUserRepository betUserRepositroy)
        {
            _embedService = embedService;
            _betUserRepository = betUserRepositroy;
        }

        [Command("serverinfo", RunMode = RunMode.Async)]
        [Alias("serverstats")]
        [Summary("Get serverinfo")]
        public async Task GetServerInfo()
        {
            var guild = Context.Guild;
            await guild.DownloadUsersAsync();

            await ReplyAsync(null, false, _embedService.EmbedServerInfo(guild).Build());
        }

        [Command("userinfo", RunMode = RunMode.Async)]
        [Alias("userstats")]
        [Summary("Get userinfo")]
        public async Task GetMyInfo()
        {
            var guild = Context.Guild;
            var user = Context.User as SocketGuildUser;
            await guild.DownloadUsersAsync();

            var pointsbetUser = await _betUserRepository.GetBetUser(user.Id);

            await ReplyAsync(null, false, _embedService.EmbedMyInfo(guild, user, pointsbetUser).Build());
        }

    }
}

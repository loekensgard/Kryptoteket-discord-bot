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
        private readonly IBetWinnersRepository _betWinnersRepository;

        public ServerCommands(EmbedService embedService, IBetWinnersRepository betWinnersRepository)
        {
            _embedService = embedService;
            _betWinnersRepository = betWinnersRepository;
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
        [Summary("Get serverinfo")]
        public async Task GetMyInfo()
        {
            //var guild = Context.Guild;
            //var user = Context.User as SocketGuildUser;
            //await guild.DownloadUsersAsync();

            //var points = await _betWinnersRepository.GetBetWinner(user.Id.ToString() + "bet");

            //await ReplyAsync(null, false, _embedService.EmbedMyInfo(guild, user, points).Build());
        }

    }
}

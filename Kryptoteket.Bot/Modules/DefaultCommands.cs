using Discord;
using Discord.Commands;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Modules
{
    [Name("DefaultCommands")]
    public class DefaultCommands : ModuleBase<SocketCommandContext>
    {
        private readonly EmbedService _embedService;
        private readonly IBetUserRepository _betUserRepository;
        private readonly IPlacedUserBetRepository _placedUserBetRepository;

        public DefaultCommands(EmbedService embedService, IBetUserRepository betUserRepository, IPlacedUserBetRepository placedUserBetRepository)
        {
            _embedService = embedService;
            _betUserRepository = betUserRepository;
            _placedUserBetRepository = placedUserBetRepository;
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

        [Command("hello", RunMode = RunMode.Async)]
        public async Task CheckIfUp()
        {
            var user = Context.User;
            await ReplyAsync($"Hello {user.Username}", false, null);
        }

        [RequireUserPermission(GuildPermission.BanMembers)]
        [Command("updateusers", RunMode = RunMode.Async)]
        [Summary("Update usernames")]
        public async Task UpdateUserNames()
        {
            var placedUsers = await _placedUserBetRepository.GetAllPlacedBets();
            var betUsers = await _betUserRepository.GetUsers();

            var guild = Context.Guild;
            await guild.DownloadUsersAsync();

            var users = guild.Users;

            var sb = new StringBuilder();

            foreach(var user in betUsers)
            {
                var holdName = user.Name;
                var updated = false;
                var guildUser = users.FirstOrDefault(x => x.Id == user.BetUserId);
                if (guildUser != null && guildUser.Username != user.Name)
                {
                    await _betUserRepository.UpdateName(user.BetUserId, guildUser.Username);
                    updated = true;
                }

                var placedBetUser = placedUsers.FirstOrDefault(x => x.BetUserId == user.BetUserId);
                if (guildUser != null && guildUser.Username != placedBetUser.Name)
                {
                    await _placedUserBetRepository.UpdateName(user.BetUserId, guildUser.Username);
                    updated = true;
                }

                if (updated) sb.AppendLine($"{holdName} -> {guildUser.Username}");
            }

            await ReplyAsync(sb.ToString());
        }
    }
}

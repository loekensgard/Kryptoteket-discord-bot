using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Services;
using System;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Modules
{
    [Name("CovidCommands")]
    public class ReflinksCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IReflinkRepository _reflinkRepository;
        private readonly EmbedService _embedService;

        public ReflinksCommands(IReflinkRepository reflinkRepository, EmbedService embedService)
        {
            _reflinkRepository = reflinkRepository;
            _embedService = embedService;
        }

        [Command("reflink", RunMode = RunMode.Async)]
        [Summary("Get random reflink")]
        public async Task GetReflink()
        {
            var guild = Context.Guild as SocketGuild;

            var reflinks = await _reflinkRepository.GetReflinks(guild.Id, true);

            if(reflinks.Count == 0) { await ReplyAsync($"Could not find any reflinks"); return; }

            var random = new Random();
            var index = random.Next(reflinks.Count);

            var luckyLink = reflinks[index];
            await ReplyAsync($"<{luckyLink.Link}>");
        }

        [Command("addref", RunMode = RunMode.Async)]
        [Summary("Add reflink")]
        public async Task RequestReflink(string reflink)
        {
            var user = Context.User as SocketGuildUser;
            var guild = Context.Guild as SocketGuild;

            if (!reflink.Contains("https://miraiex.com/affiliate/?referral=")) { await ReplyAsync($"Reflink is wrong format"); return; }
            if (await _reflinkRepository.Exists(user.Id, guild.Id)) { await ReplyAsync($"Reflink is already in list"); return; }

            await _reflinkRepository.AddReflink(user.Id, user.Username, reflink.Trim(), guild.Id);
            await ReplyAsync($"Reflink was submitted for approval");
        }

        [Command("getrefs", RunMode = RunMode.Async)]
        [Summary("Request all reflinks")]
        public async Task GetReflinks()
        {
            var guild = Context.Guild as SocketGuild;

            var reflinks = await _reflinkRepository.GetReflinks(guild.Id);
            await ReplyAsync(null, false, _embedService.EmbedAllReflinks(reflinks).Build());
        }

        [Command("getref", RunMode = RunMode.Async)]
        [Summary("Request your reflink")]
        public async Task GetYourReflink(SocketGuildUser user = null)
        {
            if(user == null) user = Context.User as SocketGuildUser;
            var guild = Context.Guild as SocketGuild;

            var reflink = await _reflinkRepository.GetReflink(user.Id, guild.Id);
            if (reflink != null)
            {
                await ReplyAsync(null, false, _embedService.EmbedOwnRef(reflink).Build());
            }
            else
            {
                await ReplyAsync($"Could not find your reflink");
            }
        }

        [Command("updateref", RunMode = RunMode.Async)]
        [Summary("Update your reflink")]
        public async Task UpdateYourReflink(string reflink)
        {
            var user = Context.User as SocketGuildUser;
            var guild = Context.Guild as SocketGuild;

            if (!reflink.Contains("https://miraiex.com/affiliate/?referral=")) { await ReplyAsync($"Reflink is wrong format"); return; }

            await _reflinkRepository.UpdateReflink(user.Id, reflink, guild.Id);

            await ReplyAsync($"Reflink was updated");
        }

        [Command("deleteref", RunMode = RunMode.Async)]
        [Summary("Delete your reflink")]
        public async Task DeleteYourReflink()
        {
            var user = Context.User as SocketGuildUser;
            var guild = Context.Guild as SocketGuild;

            await _reflinkRepository.DeleteReflink(user.Id, guild.Id);

            await ReplyAsync($"Reflink was deleted");
        }

        [Command("approve", RunMode = RunMode.Async)]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [Summary("Approve reflink")]
        public async Task ApproveRef(SocketGuildUser user)
        {
            if (user == null) { await ReplyAsync($"Found no user"); return; }
            var guild = Context.Guild as SocketGuild;

            var reflink = await _reflinkRepository.GetReflink(user.Id, guild.Id);

            if (!reflink.Approved)
            {
                reflink.Approved = true;
                await _reflinkRepository.Update(user.Id, reflink, guild.Id);
            }
        }

        [Command("reject", RunMode = RunMode.Async)]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [Summary("Reject reflink")]
        public async Task RejectRef(SocketGuildUser user)
        {
            if (user == null) { await ReplyAsync($"Found no user"); return; }
            var guild = Context.Guild as SocketGuild;

            var reflink = await _reflinkRepository.GetReflink(user.Id, guild.Id);

            if (reflink.Approved)
            {
                reflink.Approved = false;
                await _reflinkRepository.Update(user.Id, reflink, guild.Id);
            }
        }

    }
}

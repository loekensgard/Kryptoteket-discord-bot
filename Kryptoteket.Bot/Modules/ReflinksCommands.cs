using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Modules
{
    [Name("CovidCommands")]
    public class ReflinksCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IRefUserRepository _reflinkRepository;
        private readonly EmbedService _embedService;
        private readonly IRefExchangeRepository _refExchangeRepository;

        public ReflinksCommands(IRefUserRepository reflinkRepository, EmbedService embedService, IRefExchangeRepository refExchangeRepository)
        {
            _reflinkRepository = reflinkRepository;
            _embedService = embedService;
            _refExchangeRepository = refExchangeRepository;
        }

        [Command("reflink", RunMode = RunMode.Async)]
        [Summary("Get random reflink")]
        public async Task GetReflink(string exchange)
        {
            var reflinks = await _refExchangeRepository.GetRefExchanges(exchange: exchange);

            if (reflinks.Count == 0) { await ReplyAsync($"Could not find any reflinks"); return; }

            var random = new Random();
            var index = random.Next(reflinks.Count);

            var luckyLink = reflinks[index];
            await ReplyAsync($"<{luckyLink.Link}>");
        }

        [Command("addref", RunMode = RunMode.Async)]
        [Summary("Add reflink")]
        public async Task RequestReflink(string exchange, string reflink)
        {
            if (string.IsNullOrEmpty(exchange)) { await ReplyAsync($"Exchange cannot be null"); return; }
            if (string.IsNullOrEmpty(reflink)) { await ReplyAsync($"Reflink cannot be null"); return; }

            var user = Context.User as SocketGuildUser;

            var userId = $"ref{user.Id}";
            if (!await _reflinkRepository.Exists(userId))
            {
                await _reflinkRepository.CreateRefUser(userId, user.Username);
            }

            var identifier = exchange.Substring(0, 4).ToLower();
            if (await _refExchangeRepository.Exists($"{identifier}{user.Id}")) { await ReplyAsync($"Reflink is already in list"); return; }

            await _refExchangeRepository.CreateReflink($"{identifier}{user.Id}", exchange, reflink.Trim(), userId);
            await ReplyAsync($"Reflink was submitted for approval");
        }

        [Command("getrefs", RunMode = RunMode.Async)]
        [Summary("Request all reflinks")]
        public async Task GetReflinks(string exchange = null)
        {
            var refusers = await _reflinkRepository.GetRefUsers(exchange);
            var reflinks = await _refExchangeRepository.GetRefExchanges(exchange);
            await ReplyAsync(null, false, _embedService.EmbedAllReflinks(refusers, reflinks).Build());
        }

        [Command("getref", RunMode = RunMode.Async)]
        [Summary("Request your reflink")]
        public async Task GetYourReflink(SocketGuildUser user = null)
        {
            if (user == null) user = Context.User as SocketGuildUser;

            var refuser = await _reflinkRepository.GetRefUser($"ref{user.Id}");
            if (refuser == null) { await ReplyAsync($"Could not find your reflinks"); return; }

            var refExchanges = await _refExchangeRepository.GetRefExchanges(userId: refuser.id);

            if(refExchanges.Count == 0) { await ReplyAsync($"Could not find your reflinks"); return; }

            await ReplyAsync(null, false, _embedService.EmbedOwnRef(refExchanges).Build());
        }

        [Command("updateref", RunMode = RunMode.Async)]
        [Summary("Update your reflink")]
        public async Task UpdateYourReflink(string exchange, string reflink)
        {
            var user = Context.User as SocketGuildUser;

            var refuser = await _reflinkRepository.GetRefUser($"ref{user.Id}");
            if (refuser == null) { await ReplyAsync($"Could not find your reflinks"); return; }


            var refexchangeId = $"{exchange.Substring(0, 4).ToLower()}{user.Id}";
            var refExchange = await _refExchangeRepository.GetRefExchange(refexchangeId);

            if (refExchange.Link != reflink)
                refExchange.Link = reflink;

            await _refExchangeRepository.UpdateRefExchanges(refexchangeId, refExchange);
            await ReplyAsync($"Reflink was updated");
        }

        [Command("deleteref", RunMode = RunMode.Async)]
        [Summary("Delete your reflink")]
        public async Task DeleteYourReflink(string exchange, SocketGuildUser guildUser = null)
        {
            var user = Context.User as SocketGuildUser;

            if (guildUser != null)
            {
                var approver = Context.User as SocketGuildUser;
                if (!approver.GuildPermissions.KickMembers && approver.Id != 396311377247207434) { await ReplyAsync($"You don't have permissions to do that"); return; }

                user = guildUser;
            }

            var refuser = await _reflinkRepository.GetRefUser($"ref{user.Id}");
            if (refuser == null) { await ReplyAsync($"Could not find your reflinks"); return; }

            await _refExchangeRepository.DeleteReflink($"{exchange.Substring(0, 4).ToLower()}{user.Id}");
            await ReplyAsync($"Reflink was deleted");
        }

        [Command("approve", RunMode = RunMode.Async)]
        [Summary("Approve reflink")]
        public async Task ApproveRef(SocketGuildUser user)
        {
            var approver = Context.User as SocketGuildUser;
            if (!approver.GuildPermissions.KickMembers && approver.Id != 396311377247207434) { await ReplyAsync($"You don't have permissions to do that"); return; }
            if (user == null) { await ReplyAsync($"Found no user"); return; }

            var refuserId = $"ref{user.Id}";
            var reflink = await _reflinkRepository.GetRefUser(refuserId);

            if (!reflink.Approved)
            {
                reflink.Approved = true;
                await _reflinkRepository.Update(refuserId, reflink);
                await ReplyAsync($"{user.Username} is approved");
            }
            else
            {
                await ReplyAsync($"{user.Username} is already approved");
            }
        }

        [Command("reject", RunMode = RunMode.Async)]
        [Summary("Reject reflink")]
        public async Task RejectRef(SocketGuildUser user)
        {
            var approver = Context.User as SocketGuildUser;
            if (!approver.GuildPermissions.KickMembers && approver.Id != 396311377247207434) { await ReplyAsync($"You don't have permissions to do that"); return; }

            if (user == null) { await ReplyAsync($"Found no user"); return; }

            var refuserId = $"ref{user.Id}";
            var reflink = await _reflinkRepository.GetRefUser(refuserId);

            if (reflink.Approved)
            {
                reflink.Approved = false;
                await _reflinkRepository.Update(refuserId, reflink);
                await ReplyAsync($"{user.Username} is removed from the approved list");
            }
            else
            {
                await ReplyAsync($"{user.Username} is already removed from the approved list");
            }
        }

        [Command("fix", RunMode = RunMode.Async)]
        public async Task RejectRef()
        {

            var allu = await _reflinkRepository.GetRefUsers();
            
            foreach(var u in allu)
            {
                if (!u.Approved)
                {
                    u.Approved = true;
                    await _reflinkRepository.Update(u.id, u);
                }
            }

        }

    }
}

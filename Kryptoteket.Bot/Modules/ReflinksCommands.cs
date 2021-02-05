using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models.Reflinks;
using Kryptoteket.Bot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Modules
{
    [Name("CovidCommands")]
    public class ReflinksCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IRefUserRepository _refUserRepository;
        private readonly EmbedService _embedService;
        private readonly IRefExchangeRepository _refExchangeRepository;
        private readonly IReflinkRepository _reflinkRepository;

        public ReflinksCommands(IRefUserRepository refUserRepository, EmbedService embedService, IRefExchangeRepository refExchangeRepository, IReflinkRepository reflinkRepository)
        {
            _refUserRepository = refUserRepository;
            _embedService = embedService;
            _refExchangeRepository = refExchangeRepository;
            _reflinkRepository = reflinkRepository;
        }

        //COMMANDS FOR ADMINISTARTION OF REFLINK EXCHANGES
        [Command("addex", RunMode = RunMode.Async)]
        [Summary("Add reflink")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task AddRefExchange(string exchange, string inputEmoji)
        {
            if (string.IsNullOrEmpty(exchange)) { await ReplyAsync($"Exchange cannot be null"); return; }
            if (string.IsNullOrEmpty(inputEmoji)) { await ReplyAsync($"Emoji cannot be null"); return; }

            var stripped = Regex.Replace(inputEmoji, "[^0-9]", "");
            if(string.IsNullOrEmpty(stripped)) { await ReplyAsync($"Default emotes are not valid"); return; }

            var emoji = await Context.Guild.GetEmoteAsync(ulong.Parse(stripped));

            if(emoji == null) { await ReplyAsync($"Could not find emoji in Guild emotes"); return; }
            if (!await _refExchangeRepository.Exists(exchange))
            {
                await _refExchangeRepository.CreateRefExchange(new RefExchange
                {
                    Name = exchange.ToLower(),
                    EmojiId = emoji.Id
                });
                await ReplyAsync($"Exchange added");
            }
            else
            {
                await ReplyAsync($"Exchange already exists");
            }
        }

        [Command("approve", RunMode = RunMode.Async)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [Summary("Approve reflink")]
        public async Task ApproveRef(SocketGuildUser user)
        {
            if (user == null) { await ReplyAsync($"Found no user"); return; }
            var refuser = await _refUserRepository.GetRefUser(user.Id);

            if (!refuser.Approved)
            {
                refuser.Approved = true;
                await _refUserRepository.UpdateUser(refuser);
                await ReplyAsync($"{user.Username} is approved");
            }
            else
            {
                await ReplyAsync($"{user.Username} is already approved");
            }
        }

        [Command("reject", RunMode = RunMode.Async)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [Summary("Reject reflink")]
        public async Task RejectRef(SocketGuildUser user)
        {
            if (user == null) { await ReplyAsync($"Found no user"); return; }
            var refuser = await _refUserRepository.GetRefUser(user.Id);

            if (refuser.Approved)
            {
                refuser.Approved = false;
                await _refUserRepository.UpdateUser(refuser);
                await ReplyAsync($"{user.Username} is rejected");
            }
            else
            {
                await ReplyAsync($"{user.Username} is already rejected");
            }
        }
        //END


        [Command("reflink", RunMode = RunMode.Async)]
        [Alias("ref", "referal")]
        [Summary("Get random reflink")]
        public async Task GetReflink(string exchange = null)
        {
            var exchanges = await _refExchangeRepository.GetRefExchanges(exchange);
            if (exchanges.Count == 0) { await ReplyAsync($"Could not find any reflinks"); return; }

            if(exchanges.Count >= 2)
            {
                var emotes = Context.Guild.Emotes;
                var sent = await ReplyAsync(null, false, _embedService.CreateReactMessage(exchanges, emotes).Build());
                await sent.AddReactionsAsync(emotes.Join(exchanges, em => em.Id, ex => ex.EmojiId, (em, ex) => em).ToArray());
                return;
            }

            var links = exchanges.SelectMany(x => x.Reflinks);

            var random = new Random();
            var index = random.Next(links.Count());

            var luckyLink = links.ElementAt(index);
            await ReplyAsync($"<{luckyLink.Link}>");
        }


        [Command("addref", RunMode = RunMode.Async)]
        [Summary("Add reflink")]
        public async Task RequestReflink(string exchange, string reflink)
        {
            if (string.IsNullOrEmpty(exchange)) { await ReplyAsync($"Exchange cannot be null"); return; }
            if (string.IsNullOrEmpty(reflink)) { await ReplyAsync($"Reflink cannot be null"); return; }

            var refexch = await _refExchangeRepository.GetRefExchange(exchange);
            if (refexch == null) { await ReplyAsync("https://tenor.com/view/we-dont-do-that-here-black-panther-tchalla-bruce-gif-16558003"); return; }

            var user = Context.User as SocketGuildUser;

            var refUser = await _refUserRepository.GetRefUser(user.Id);
            if (refUser == null)
            {
                var ex = new List<RefExchange>
                {
                    refexch
                };
                refUser = await _refUserRepository.CreateRefUser(new RefUser
                {
                    RefUserId = user.Id,
                    Name = user.Username,
                    RefExchanges = ex
                });
            }

            refexch.RefUsers.Add(refUser);
            await _refExchangeRepository.UpdateExchange(refexch);

            if (await _reflinkRepository.Exist(user.Id, refexch.RefExchangeId, reflink)) { await ReplyAsync($"Reflink is already in list"); return; }
            await _reflinkRepository.CreateReflink(new RefLink { Link = reflink.Trim(), RefExchangeId = refexch.RefExchangeId, RefUserId = refUser.RefUserId, Name = refexch.Name });

            await ReplyAsync($"Reflink was added");
        }

        [Command("getrefs", RunMode = RunMode.Async)]
        [Summary("Request all reflinks")]
        public async Task GetReflinks(string exchange = null)
        {
            var refExchanges = await _refExchangeRepository.GetRefExchanges(exchange);
            if (!refExchanges.Any()) { await ReplyAsync($"Could not find any reflinks"); return; }

            await ReplyAsync(null, false, _embedService.EmbedAllReflinks(refExchanges).Build());
        }

        [Command("getref", RunMode = RunMode.Async)]
        [Summary("Request your reflink")]
        public async Task GetYourReflink(SocketGuildUser user = null)
        {
            if (user == null) user = Context.User as SocketGuildUser;

            var refuser = await _refUserRepository.GetRefUser(user.Id);
            if (refuser == null) { await ReplyAsync($"Could not find your reflinks"); return; }

            if (refuser.Reflinks.Count == 0) { await ReplyAsync($"Could not find your reflinks"); return; }
            await ReplyAsync(null, false, _embedService.EmbedOwnRef(refuser).Build());
        }

        [Command("updateref", RunMode = RunMode.Async)]
        [Summary("Update your reflink")]
        public async Task UpdateYourReflink(string exchange, string reflink)
        {
            var user = Context.User as SocketGuildUser;

            var refuser = await _refUserRepository.GetRefUser(user.Id);
            if (refuser == null) { await ReplyAsync($"Could not find your reflinks"); return; }

            var link = refuser.Reflinks.FirstOrDefault(x => x.Name.ToLower() == exchange.ToLower().Trim());
            if (link == null) { await ReplyAsync($"Could not find your reflinks"); return; }

            if (link.Link != reflink)
                link.Link = reflink;

            await _reflinkRepository.UpdateReflink(link);
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

            var refuser = await _refUserRepository.GetRefUser(user.Id);
            if (refuser == null) { await ReplyAsync($"Could not find your reflinks"); return; }

            var link = refuser.Reflinks.FirstOrDefault(x => x.Name.ToLower() == exchange.ToLower().Trim());
            if (link == null) { await ReplyAsync($"Could not find your reflinks"); return; }

            await _reflinkRepository.DeleteReflink(link.Id);
            await ReplyAsync($"Reflink was deleted");
        }
    }
}

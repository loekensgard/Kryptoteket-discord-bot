using Discord.Commands;
using Discord.WebSocket;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using Kryptoteket.Bot.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Modules
{
    [Name("BetCommands")]
    public class BetCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IBetRepository _betRepository;
        private readonly IUserBetRepository _userBetRepository;
        private readonly EmbedService _embedService;

        public BetCommands(IBetRepository betRepository, IUserBetRepository userBetRepository, EmbedService embedService)
        {
            _betRepository = betRepository;
            _userBetRepository = userBetRepository;
            _embedService = embedService;
        }

        [Command("addbet", RunMode = RunMode.Async)]
        [Summary("add a bet")]
        public async Task Addbet(string shortName, string date)
        {
            var approver = Context.User as SocketGuildUser;
            if (!approver.GuildPermissions.KickMembers && approver.Id != 396311377247207434) { await ReplyAsync($"You don't have permissions to do that"); return; }

            DateTimeOffset dateDTO;
            if (!DateTimeOffset.TryParse(date, CultureInfo.GetCultureInfo("nb-NO"), DateTimeStyles.None, out dateDTO)) { await ReplyAsync($"Dateformat is incorrect"); return; }

            var bet = new Bet
            {
                id = shortName.ToLower().Trim(),
                Date = dateDTO,
                ShortName = shortName,
                AddedBy = approver.Username,
                Users = new List<UserBet>()
            };

            try
            {
                await _betRepository.CreateBet(bet);
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
                await ReplyAsync($"Bet already exists");
                return;
            }
            await ReplyAsync($"{shortName} added");
        }

        [Command("deletebet", RunMode = RunMode.Async)]
        [Summary("Delete a bet")]
        public async Task DeleteBet(string shortName)
        {
            var approver = Context.User as SocketGuildUser;
            if (!approver.GuildPermissions.KickMembers && approver.Id != 396311377247207434) { await ReplyAsync($"You don't have permissions to do that"); return; }

            await _betRepository.DeleteBet(shortName.ToLower().Trim());

            await ReplyAsync($"{shortName} deleted");
        }

        [Command("bet", RunMode = RunMode.Async)]
        [Summary("Bet on a bet")]
        public async Task Bet(string shortName, string price)
        {
            var user = Context.User as SocketGuildUser;
            var bet = await _betRepository.Getbet(shortName.ToLower().Trim());
            if (bet == null) { await ReplyAsync($"Bet Doesn't exist"); return; }

            if (!int.TryParse(price.Trim(), out int priceDTO)) { await ReplyAsync($"Price is incorrect"); return; }

            var userBet = new UserBet
            {
                Price = price.Trim(),
                id = bet.id + user.Id.ToString(),
                Name = user.Username,
                BetId = bet.id,
                BetPlaced = DateTimeOffset.Now
            };

            try
            {
                await _userBetRepository.AddUserBet(userBet);
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
                await ReplyAsync($"Bet already exists");
                return;
            }
            await ReplyAsync($"**{userBet.Name}** | Price: ${priceDTO:#,##0} at {bet.Date.ToString("dd/M/yyyy", CultureInfo.GetCultureInfo("nb-NO"))}");
        }

        [Command("getbet", RunMode = RunMode.Async)]
        [Summary("Get a bet")]
        public async Task GetBet(string shortName)
        {
            var bet = await _betRepository.Getbet(shortName.Trim().ToLower());
            if (bet == null) { await ReplyAsync($"Bet doesn't exist"); return; }
            var userbets = await _userBetRepository.GetUserBets(bet.id);

            if (userbets.Count == 0) { await ReplyAsync($"No bets are placed"); return; }

            await ReplyAsync(null, false, _embedService.EmbedBets(bet).Build());
        }
    }
}

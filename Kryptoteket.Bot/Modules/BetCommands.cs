using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Kryptoteket.Bot.Exceptions;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using Kryptoteket.Bot.Models.Bets;
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
        private readonly IPlacedUserBetRepository _placedUserBetRepository;
        private readonly EmbedService _embedService;
        private readonly IBetUserRepository _betUserRepository;

        public BetCommands(IBetRepository betRepository, IPlacedUserBetRepository placedUserBetRepository, EmbedService embedService, IBetUserRepository betUserRepository)
        {
            _betRepository = betRepository;
            _placedUserBetRepository = placedUserBetRepository;
            _embedService = embedService;
            _betUserRepository = betUserRepository;
        }

        [Command("addbet", RunMode = RunMode.Async)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [Summary("add a bet")]
        public async Task Addbet(string shortName, string date)
        {
            DateTimeOffset dateDTO;
            if (!DateTimeOffset.TryParse(date, CultureInfo.GetCultureInfo("nb-NO"), DateTimeStyles.None, out dateDTO)) { await ReplyAsync($"Dateformat is incorrect"); return; }

            var user = Context.User as SocketGuildUser;

            var bet = new Bet
            {
                Date = dateDTO,
                ShortName = shortName.ToLower().Trim(),
                AddedBy = user.Username
            };

            await _betRepository.CreateBet(bet);
            await ReplyAsync($"{shortName} added");
        }

        [Command("deletebet", RunMode = RunMode.Async)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [Summary("Delete a bet")]
        public async Task DeleteBet(string shortName)
        {
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

            var exists = await _placedUserBetRepository.GetPlacedBet(bet.BetId, user.Id);
            if(exists) { await ReplyAsync($"You can't bet twice"); return; }

            var userExists = await _betUserRepository.GetBetUser(user.Id);
            if (userExists == null) await _betUserRepository.AddBetUser(new BetUser { BetUserId = user.Id, Name = user.Username, Points = 0 });

            var userBet = new PlacedBet
            {
                Price = priceDTO,
                Name = user.Username,
                BetId = bet.BetId,
                BetUserId = user.Id,
                BetPlaced = DateTimeOffset.Now
            };

            await _placedUserBetRepository.AddPlacedBet(userBet);
            await ReplyAsync($"**{userBet.Name}** | Price: ${priceDTO:#,##0} at {bet.Date.ToString("dd/M/yyyy", CultureInfo.GetCultureInfo("nb-NO"))}");
        }

        [Command("getbet", RunMode = RunMode.Async)]
        [Summary("Get a bet")]
        public async Task GetBet(string shortName)
        {
            var bet = await _betRepository.Getbet(shortName.Trim().ToLower());
            if (bet == null) { await ReplyAsync($"Bet doesn't exist"); return; }
            if (bet.PlacedBets.Count == 0) { await ReplyAsync($"No bets are placed"); return; }

            await ReplyAsync(null, false, _embedService.EmbedBets(bet).Build());
        }
    }
}

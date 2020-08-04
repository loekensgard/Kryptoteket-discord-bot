using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using Kryptoteket.Bot.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Modules
{
    [Name("TickerCheckCommands")]
    public class TickerCheckCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IMiraiexService _miraiexService;
        private readonly EmbedService _embedService;

        public TickerCheckCommands(IMiraiexService miraiexService, EmbedService embedService)
        {
            _miraiexService = miraiexService;
            _embedService = embedService;
        }

        [Command("ticker btcnok", RunMode = RunMode.Async)]
        [Summary("Get btcnok ticker from Miraiex")]
        public async Task GetBTCTickerMiraiex()
        {
            var ticker = await _miraiexService.GetTicker("btcnok");
            var builder = _embedService.EmbedTicker("btcnok", ticker);

            await ReplyAsync(null, false, builder.Build());
        }


        [Command("ticker ethnok", RunMode = RunMode.Async)]
        [Summary("Get ethnok ticker from Miraiex")]
        public async Task GetETHTickerMiraiex()
        {
            var ticker = await _miraiexService.GetTicker("ethnok");
            var builder = _embedService.EmbedTicker("ethnok", ticker);

            await ReplyAsync(null, false, builder.Build());
        }

    }
}

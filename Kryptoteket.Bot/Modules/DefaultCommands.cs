using Discord.Commands;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Services;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Modules
{
    [Name("DefaultCommands")]
    public class DefaultCommands : ModuleBase<SocketCommandContext>
    {
        private readonly EmbedService _embedService;
        private readonly IReflinkRepository _reflinkRepository;

        public DefaultCommands(EmbedService embedService, IReflinkRepository reflinkRepository)
        {
            _embedService = embedService;
            _reflinkRepository = reflinkRepository;
        }

        [Command("help", RunMode = RunMode.Async)]
        [Summary("Get help")]
        public async Task GetHelpText()
        {
            await ReplyAsync(null, false, _embedService.Embedhelp().Build());
        }

        [Command("reflink", RunMode = RunMode.Async)]
        [Summary("Get random reflink")]
        public async Task GetReflink()
        {
            var reflinks = await _reflinkRepository.GetReflinks();

            var random = new Random();
            var index = random.Next(reflinks.Count);

            var luckyLink = reflinks[index];

            await ReplyAsync(luckyLink);
        }

        [Command("support", RunMode = RunMode.Async)]
        [Summary("Support the developer")]
        public async Task GetSupportText()
        {
            await ReplyAsync(null, false, _embedService.EmbedSupportMe().Build());
        }


    }
}

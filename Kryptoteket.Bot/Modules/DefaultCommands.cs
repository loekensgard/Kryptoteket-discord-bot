using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Services;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            await ReplyAsync($"<{luckyLink}>");
        }

        [Command("addref", RunMode = RunMode.Async)]
        [Summary("Add reflinks to the DB")]
        public async Task AddReflink(string name, string reflink)
        {
            if (reflink.Contains("https://miraiex.com/affiliate/?referral=")) {
                var user = Context.User as SocketGuildUser;
                var allowed = user.Roles.Any(x => x.Id == 344896780657491968 || x.Id == 344896529707958272);

                if (allowed)
                {
                    if (await _reflinkRepository.AddReflink(name.Trim(), reflink.Trim())) await ReplyAsync($"Reflink was added");
                    else await ReplyAsync($"Reflink was a duplicate");
                }
                else
                {
                    await ReplyAsync($"You don't have permissions to add reflinks");
                }
            }

        }

        [Command("support", RunMode = RunMode.Async)]
        [Summary("Support the developer")]
        public async Task GetSupportText()
        {
            await ReplyAsync(null, false, _embedService.EmbedSupportMe().Build());
        }


    }
}

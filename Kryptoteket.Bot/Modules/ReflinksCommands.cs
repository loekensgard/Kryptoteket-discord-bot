using Discord.Commands;
using Discord.WebSocket;
using Kryptoteket.Bot.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Modules
{
    [Name("CovidCommands")]
    public class ReflinksCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IReflinkRepository _reflinkRepository;

        public ReflinksCommands(IReflinkRepository reflinkRepository)
        {
            _reflinkRepository = reflinkRepository;
        }

        [Command("reflink", RunMode = RunMode.Async)]
        [Summary("Get random reflink")]
        public async Task GetReflink()
        {
            var reflinks = await _reflinkRepository.GetReflinks();

            var random = new Random();
            var index = random.Next(reflinks.Count);

            var luckyLink = reflinks[index];
            await ReplyAsync($"<{luckyLink.Link}>");
        }

        [Command("addref", RunMode = RunMode.Async)]
        [Summary("Request own reflink")]
        public async Task RequestReflink(string reflink)
        {
            var user = Context.User as SocketGuildUser;

            if (!reflink.Contains("https://miraiex.com/affiliate/?referral=")){ await ReplyAsync($"Reflink is wrong format");return;}
            if (await _reflinkRepository.Exists(user.Id)) { await ReplyAsync($"Reflink is already in list"); return;}

            await _reflinkRepository.AddReflink(user.Id, user.Username, reflink.Trim());
            await ReplyAsync($"Reflink was submitted for approval");
        }

        [Command("getref", RunMode = RunMode.Async)]
        [Summary("Request own reflink")]
        public async Task GetReflinks()
        {
            await ReplyAsync($"Method not implemented");

            var user = Context.User as SocketGuildUser;
            var allowed = user.Roles.Any(x => x.Id == 344896780657491968 || x.Id == 344896529707958272);

            if(!allowed) { await ReplyAsync($"Insufficient permissions"); return; }

            var reflinks = await _reflinkRepository.GetReflinks();

        }

    }
}

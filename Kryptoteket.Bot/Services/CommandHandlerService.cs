using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Kryptoteket.Bot.Configurations;
using Kryptoteket.Bot.Interfaces;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Services
{
    public class CommandHandlerService
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly CommandService _commandService;
        private readonly IServiceProvider _services;
        private readonly IRefExchangeRepository _refExchangeRepository;
        private readonly DiscordConfiguration _discordOptions;
        private const string _messageErrorTemplate = "Discord Error {reasonType} {reasonDescription} {message}";

        public CommandHandlerService(
            DiscordSocketClient discordSocketClient,
            CommandService commandService,
            IServiceProvider services,
            IOptions<DiscordConfiguration> discordOptions,
            IRefExchangeRepository refExchangeRepository)
        {
            _discordSocketClient = discordSocketClient;
            _commandService = commandService;
            _services = services;
            _refExchangeRepository = refExchangeRepository;
            _discordOptions = discordOptions.Value;

            _discordSocketClient.MessageReceived += OnMessageReceivedAsync;
            _discordSocketClient.ReactionAdded += OnMessageReactionAdd;
            _discordSocketClient.Ready += ReadyAsync;
        }

        private async Task OnMessageReactionAdd(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            try
            {
                if (!message.HasValue) return;
                if (message.HasValue && message.Value.Source != MessageSource.Bot) return;
                if (reaction.User.Value.IsBot) return;

                var chnl = channel as SocketGuildChannel;
                var guildEmojies = chnl.Guild.Emotes;

                if (!guildEmojies.Any(x => x.Name == reaction.Emote.Name)) return;

                var emote = reaction.Emote as Emote;
                var emojiFromExchange = await _refExchangeRepository.GetRefExchangeFromEmoji(emote.Id);

                if (emojiFromExchange == null) return;

                var links = emojiFromExchange.Reflinks;

                if (!links.Any()) return;

                var random = new Random();
                var index = random.Next(links.Count);

                var luckyLink = links.ElementAt(index);

                var user = await channel.GetUserAsync(reaction.UserId);
                await channel.SendMessageAsync($"{user.Mention}: <{luckyLink.Link}>");
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed Excetuing command async");
            }
        }

        private Task ReadyAsync()
        {
            Log.Information("{user} is connected!", _discordSocketClient.CurrentUser);
            return Task.CompletedTask;
        }

        private async Task OnMessageReceivedAsync(SocketMessage parameterMessage)
        {
            // Don't handle the command if it is a system message
            var message = parameterMessage as SocketUserMessage;
            if (message == null) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_discordSocketClient, message);

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix(_discordOptions.Prefix, ref argPos) ||
                message.HasMentionPrefix(_discordSocketClient.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
            {
                return;
            }

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            try
            {
                var result = await _commandService.ExecuteAsync(
                    context: context,
                    argPos: argPos,
                    services: _services);

                if (!result.IsSuccess)
                    Log.Error(_messageErrorTemplate, result.Error, result.ErrorReason, message);

            }
            catch (Exception e)
            {
                Log.Error(e, "Failed Excetuing command async");
            }
        }
    }

}

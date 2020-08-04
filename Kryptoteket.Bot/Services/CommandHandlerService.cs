using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Services
{
    public class CommandHandlerService
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly CommandService _commandService;
        private readonly IServiceProvider _services;

        public CommandHandlerService(DiscordSocketClient discordSocketClient, CommandService commandService, IServiceProvider services)
        {
            _discordSocketClient = discordSocketClient;
            _commandService = commandService;
            _services = services;

            _discordSocketClient.MessageReceived += OnMessageReceivedAsync;
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
            if (!(message.HasCharPrefix('!', ref argPos) ||
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
                    Console.WriteLine("Could not send error");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

}

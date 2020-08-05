using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Services
{
    public class LoggingService
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly CommandService _commandService;

        public LoggingService(DiscordSocketClient discordSocketClient, CommandService commandService)
        {
            _discordSocketClient = discordSocketClient;
            _commandService = commandService;

            _discordSocketClient.Log += LogAsync;
            _commandService.Log += LogAsync;
        }

        private Task LogAsync(LogMessage msg)
        {
            var logText = $"{DateTime.UtcNow.ToString("hh:mm:ss")} [{msg.Severity}] {msg.Source}: {msg.Exception?.ToString() ?? msg.Message}";
            return Console.Out.WriteLineAsync(logText); 
        }

    }
}

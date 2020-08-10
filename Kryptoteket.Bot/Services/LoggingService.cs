using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;
using Serilog.Events;
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
        private const string _messageTemplate = "Discord Log {date} {severity} {source} {message}";

        public LoggingService(DiscordSocketClient discordSocketClient, CommandService commandService)
        {
            _discordSocketClient = discordSocketClient;
            _commandService = commandService;

            _discordSocketClient.Log += LogAsync;
            _commandService.Log += LogAsync;
        }

        private Task LogAsync(LogMessage msg)
        {
            var level = msg.Exception == null ? LogEventLevel.Information : LogEventLevel.Error;
            Log.Write(level, _messageTemplate, DateTime.Now, msg.Severity, msg.Source , msg.Exception?.ToString() ?? msg.Message);

            return Console.Out.WriteLineAsync($"{DateTime.UtcNow.ToString("HH:mm:ss")} [{msg.Severity}] {msg.Source}: {msg.Exception?.ToString() ?? msg.Message}"); 
        }

    }
}

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Kryptoteket.Bot.Configurations;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Services
{
    public class StartupService
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly CommandService _commandService;
        private readonly DiscordConfiguration _discordOptions;
        private readonly IServiceProvider _services;

        public StartupService(IServiceProvider services, DiscordSocketClient discordSocketClient, CommandService commandService, IOptions<DiscordConfiguration> discordOptions)
        {
            _services = services;
            _discordSocketClient = discordSocketClient;
            _commandService = commandService;
            _discordOptions = discordOptions.Value;
        }

        public async Task StartAsync()
        {
            var discordToken = !string.IsNullOrEmpty(_discordOptions.Token) ? _discordOptions.Token : throw new Exception("Missing Discord Bot token");

            await _discordSocketClient.LoginAsync(TokenType.Bot, discordToken);
            await _discordSocketClient.StartAsync();

            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

    }
}

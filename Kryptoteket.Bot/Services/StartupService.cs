using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Services
{
    public class StartupService
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly CommandService _commandService;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _services;

        public StartupService(IServiceProvider services, DiscordSocketClient discordSocketClient, CommandService commandService, IConfiguration configuration)
        {
            _services = services;
            _discordSocketClient = discordSocketClient;
            _commandService = commandService;
            _configuration = configuration;
        }

        public async Task StartAsync()
        {
            var config = _configuration.GetSection("Discord");
            var discordToken = config["Token"] ?? throw new Exception("Missing Discord Bot token");

            await _discordSocketClient.LoginAsync(TokenType.Bot, discordToken);
            await _discordSocketClient.StartAsync();

            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

    }
}

using Discord.Commands;
using Discord.WebSocket;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading.Tasks;

namespace Kryptoteket.Bot
{
    public class KryptoteketBot
    {
        private IConfiguration _configuration { get; set; }

        public async Task StartAsync()
        {
            var _builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            _configuration = _builder.Build();

            var services = new ServiceCollection()
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    LogLevel = Discord.LogSeverity.Verbose,
                    MessageCacheSize = 1000
                }))
                .AddSingleton(new CommandService(new CommandServiceConfig
                {
                    DefaultRunMode = RunMode.Async,
                    LogLevel = Discord.LogSeverity.Verbose,
                    CaseSensitiveCommands = false,
                    ThrowOnError = false
                }))
                .AddSingleton(_configuration)
                .AddSingleton<CommandHandlerService>()
                .AddSingleton<StartupService>()
                .AddSingleton<LoggingService>()
                .AddSingleton<IMiraiexService, MiraiexService>()
                .AddTransient<HttpResponseService>()
                .AddTransient<EmbedService>();

            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetRequiredService<CommandHandlerService>();

            await serviceProvider.GetRequiredService<StartupService>().StartAsync();

            await Task.Delay(-1);
        }

    }
}
using Discord.Commands;
using Discord.WebSocket;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Exceptions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Kryptoteket.Bot
{
    public class Startup
    {
        private IConfiguration _configuration { get; set; }

        public Startup()
        {
            var _builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.development.json", optional: true)
                .AddEnvironmentVariables();

            _configuration = _builder.Build();
        }

        public async Task StartAsync()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console()
                .CreateLogger();

            AppDomain.CurrentDomain.UnhandledException += HandleUnhandledExceptions;

            Log.Information("Kryptoteket.Bot starting up...");

            var startup = new Startup();
            await startup.RunAsync();
        }

        public async Task RunAsync()
        {
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

            Log.Information("Kryptoteket.Bot started");
            await serviceProvider.GetRequiredService<StartupService>().StartAsync();
            await Task.Delay(-1);
        }

        private static void HandleUnhandledExceptions(object sender, UnhandledExceptionEventArgs e)
        {
            if (Log.Logger != null && e.ExceptionObject is Exception exception)
            {
                UnhandledExceptions(exception);

                if (e.IsTerminating)
                {
                    Log.CloseAndFlush();
                }
            }
        }

        private static void UnhandledExceptions(Exception e)
        {
            Log.Logger?.Error(e, ".kryptoteket bot crashed");
        }

    }
}
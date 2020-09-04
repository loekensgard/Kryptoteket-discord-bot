using Discord.Commands;
using Discord.WebSocket;
using Kryptoteket.Bot.Configurations;
using Kryptoteket.Bot.InMemoryDB;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Services;
using Kryptoteket.Bot.Services.API;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
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

            Log.Information("Kryptoteket.Bot starting up...");

            var startup = new Startup();
            await startup.RunAsync();
        }

        public async Task RunAsync()
        {
            var services = ConfigureServices();

            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetRequiredService<CommandHandlerService>();

            Log.Information("Initiating memoryDB");
            await serviceProvider.GetRequiredService<InitMemoryDB>().InitDB();

            Log.Information("Kryptoteket.Bot started");
            await serviceProvider.GetRequiredService<StartupService>().StartAsync();
            await Task.Delay(-1);
        }

        private IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = Discord.LogSeverity.Verbose,
                MessageCacheSize = 1000
            }));

            services.AddSingleton(new CommandService(new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async,
                LogLevel = Discord.LogSeverity.Verbose,
                CaseSensitiveCommands = false,
                ThrowOnError = false
            }));

            services.AddSingleton<IMiraiexAPIService, MiraiexAPIService>();
            services.AddSingleton<ICovid19APIService, Covid19APIService>();
            services.AddSingleton<ICoinGeckoAPIService, CoinGeckoAPIService>();
            services.AddSingleton<ICoinGeckoRepository, CoinGeckoRepository>();
            services.AddSingleton<IQuickchartAPIService, QuickchartAPIService>();

            services.AddSingleton<InitMemoryDB>();
            services.AddSingleton<CommandHandlerService>();
            services.AddSingleton<StartupService>();
            services.AddSingleton<LoggingService>();
            services.AddSingleton(_configuration);

            services.AddTransient<HttpResponseService>();
            services.AddTransient<EmbedService>();

            services.Configure<ExchangesConfiguration>(options => _configuration.GetSection("Exchanges").Bind(options));
            services.Configure<DiscordConfiguration>(options => _configuration.GetSection("Discord").Bind(options));
            services.Configure<CovidAPIConfiguration>(options => _configuration.GetSection("CovidAPI").Bind(options));
            services.Configure<CoinGeckoConfiguration>(options => _configuration.GetSection("CoinGecko").Bind(options));
            services.Configure<QuickchartConfiguration>(options => _configuration.GetSection("Quickchart").Bind(options));

            return services;
        }

    }
}
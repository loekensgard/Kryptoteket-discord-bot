using Discord.Commands;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Modules
{
    [Name("CovidCommands")]
    public class CovidCommands : ModuleBase<SocketCommandContext>
    {
        private readonly ICovid19APIService _covid19APIService;
        private readonly EmbedService _embedService;

        public CovidCommands(ICovid19APIService covid19APIService, EmbedService embedService)
        {
            _covid19APIService = covid19APIService;
            _embedService = embedService;
        }

        [Command("covid", RunMode = RunMode.Async)]
        [Summary("Get covid statistics")]
        public async Task GetCovidInfoByCountry([Remainder]string countryCode)
        {
            var countryAllData = await _covid19APIService.GetCountryStats(countryCode);

            if(countryAllData.Countrydata == null) await ReplyAsync($"Could not find any data with CountryCode {countryCode}", false);

            var countryData = countryAllData.Countrydata.FirstOrDefault();
            var builder = _embedService.EmbedCovidStats(
                countryData.Info.Title,
                countryData.Info.Source, 
                countryData.TotalCases, 
                countryData.TotalNewCasesToday,
                countryData.TotalDeaths,
                countryData.TotalNewDeathsToday,
                countryData.TotalRecovered);

            await ReplyAsync(null, false, builder.Build());
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Kryptoteket.Bot.Models
{
    public class CovidCountryStats
    {
        [JsonPropertyName("countrydata")]
        public List<Countrydata> Countrydata { get; set; }

        [JsonPropertyName("stat")]
        public string Stat { get; set; }
    }

    public partial class Countrydata
    {
        [JsonPropertyName("info")]
        public Info Info { get; set; }

        [JsonPropertyName("total_cases")]
        public long TotalCases { get; set; }

        [JsonPropertyName("total_recovered")]
        public long TotalRecovered { get; set; }

        [JsonPropertyName("total_unresolved")]
        public long TotalUnresolved { get; set; }

        [JsonPropertyName("total_deaths")]
        public long TotalDeaths { get; set; }

        [JsonPropertyName("total_new_cases_today")]
        public long TotalNewCasesToday { get; set; }

        [JsonPropertyName("total_new_deaths_today")]
        public long TotalNewDeathsToday { get; set; }

        [JsonPropertyName("total_active_cases")]
        public long TotalActiveCases { get; set; }

        [JsonPropertyName("total_serious_cases")]
        public long TotalSeriousCases { get; set; }

        [JsonPropertyName("total_danger_rank")]
        public long TotalDangerRank { get; set; }
    }

    public partial class Info
    {
        [JsonPropertyName("ourid")]
        public long Ourid { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("source")]
        public Uri Source { get; set; }
    }
}

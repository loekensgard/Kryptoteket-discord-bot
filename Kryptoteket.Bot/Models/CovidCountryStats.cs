using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Kryptoteket.Bot.Models
{
    public class CovidCountryStats
    {
        [JsonPropertyName("updated")]
        public long Updated { get; set; }

        [JsonPropertyName("country")]
        public string Title { get; set; }

        [JsonPropertyName("cases")]
        public long TotalCases { get; set; }

        [JsonPropertyName("todayCases")]
        public long TotalNewCasesToday { get; set; }

        [JsonPropertyName("deaths")]
        public long TotalDeaths { get; set; }

        [JsonPropertyName("todayDeaths")]
        public long TotalNewDeathsToday { get; set; }

        [JsonPropertyName("recovered")]
        public long TotalRecovered { get; set; }

    }
}

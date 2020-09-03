using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Kryptoteket.Bot.Models
{
    public class CoingGeckoSparkline
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("sparkline_in_7d")]
        public SparklineIn7D SparklineIn7D { get; set; }

        [JsonPropertyName("price_change_percentage_7d_in_currency")]
        public double PriceChangePercentage7d { get; set; }
    }

    public partial class SparklineIn7D
    {
        [JsonPropertyName("price")]
        public List<double> Price { get; set; }
    }
}

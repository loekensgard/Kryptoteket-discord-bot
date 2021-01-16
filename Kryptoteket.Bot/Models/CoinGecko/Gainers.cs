using System;
using System.Text.Json.Serialization;

namespace Kryptoteket.Bot.Models
{
    public class Gainers
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        //[JsonPropertyName("total_volume")]
        //public int? TotalVolume { get; set; }
        public double? PriceChangeInPeriod { get; set; }
        [JsonPropertyName("last_updated")]
        public DateTime LastUpdated { get; set; }

        [JsonPropertyName("price_change_percentage_1h_in_currency")]
        public double? PriceChangePercentage1HInCurrency { set { PriceChangeInPeriod = value; } }
        [JsonPropertyName("price_change_percentage_24h_in_currency")]
        public double? PriceChangePercentage24HInCurrency { set { PriceChangeInPeriod = value; } }
        [JsonPropertyName("price_change_percentage_14d_in_currency")]
        public double? PriceChangePercentage14DInCurrency { set { PriceChangeInPeriod = value; } }
        [JsonPropertyName("price_change_percentage_1y_in_currency")]
        public double? PriceChangePercentage1YInCurrency { set { PriceChangeInPeriod = value; } }
        [JsonPropertyName("price_change_percentage_200d_in_currency")]
        public double? PriceChangePercentage200DInCurrency { set { PriceChangeInPeriod = value; } }
        [JsonPropertyName("price_change_percentage_30d_in_currency")]
        public double? PriceChangePercentage30DInCurrency { set { PriceChangeInPeriod = value; } }
        [JsonPropertyName("price_change_percentage_7d_in_currency")]
        public double? PriceChangePercentage7DInCurrency { set { PriceChangeInPeriod = value; } }
    }
}

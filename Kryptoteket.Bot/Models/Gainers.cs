using System;
using System.Text.Json.Serialization;

namespace Kryptoteket.Bot.Models
{
    public class Gainers
    {
        public Gainers()
        {
            //if (PriceChangePercentage1HInCurrency.HasValue) PriceChangeInPeriod = PriceChangePercentage1HInCurrency.Value;
            //if (PriceChangePercentage24HInCurrency.HasValue) PriceChangeInPeriod = PriceChangePercentage24HInCurrency.Value;
            //if (PriceChangePercentage14DInCurrency.HasValue) PriceChangeInPeriod = PriceChangePercentage14DInCurrency.Value;
            //if (PriceChangePercentage1YInCurrency.HasValue) PriceChangeInPeriod = PriceChangePercentage1YInCurrency.Value;
            //if (PriceChangePercentage200DInCurrency.HasValue) PriceChangeInPeriod = PriceChangePercentage200DInCurrency.Value;
            //if (PriceChangePercentage30DInCurrency.HasValue) PriceChangeInPeriod = PriceChangePercentage30DInCurrency.Value;
            //if (PriceChangePercentage7DInCurrency.HasValue) PriceChangeInPeriod = PriceChangePercentage7DInCurrency.Value;
        }

        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        public double? PriceChangeInPeriod { get; set; }
        [JsonPropertyName("last_updated")]
        public DateTime LastUpdated { get; set; }

        [JsonPropertyName("price_change_percentage_1h_in_currency")]
        public double? PriceChangePercentage1HInCurrency { set { PriceChangeInPeriod = value; } }
        [JsonPropertyName("price_change_percentage_24h_in_currency")]
        public double? PriceChangePercentage24HInCurrency { set { PriceChangeInPeriod = value; } }
        [JsonPropertyName("price_change_percentage_14d_in_currency")]
        public double? PriceChangePercentage14DInCurrency {  set { PriceChangeInPeriod = value; } }
        [JsonPropertyName("price_change_percentage_1y_in_currency")]
        public double? PriceChangePercentage1YInCurrency {  set { PriceChangeInPeriod = value; } }
        [JsonPropertyName("price_change_percentage_200d_in_currency")]
        public double? PriceChangePercentage200DInCurrency {  set { PriceChangeInPeriod = value; } }
        [JsonPropertyName("price_change_percentage_30d_in_currency")]
        public double? PriceChangePercentage30DInCurrency {  set { PriceChangeInPeriod = value; } }
        [JsonPropertyName("price_change_percentage_7d_in_currency")]
        public double? PriceChangePercentage7DInCurrency {  set { PriceChangeInPeriod = value; } }
    }
}

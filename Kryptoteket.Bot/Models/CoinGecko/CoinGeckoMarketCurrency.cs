using System.Text.Json.Serialization;

namespace Kryptoteket.Bot.Models
{
    public class CoinGeckoMarketCurrency
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("current_price")]
        public double CurrentPrice { get; set; }
        [JsonPropertyName("high_24h")]
        public double High24H { get; set; }
        [JsonPropertyName("low_24h")]
        public double Low24H { get; set; }
        [JsonPropertyName("price_change_24h")]
        public double PriceChange24H { get; set; }
        [JsonPropertyName("price_change_percentage_24h")]
        public double PriceChangePercentage24H { get; set; }
        [JsonPropertyName("ath")]
        public double Ath { get; set; }
    }
}

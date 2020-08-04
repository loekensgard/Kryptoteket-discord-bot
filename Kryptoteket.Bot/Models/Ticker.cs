using System.Text.Json.Serialization;

namespace Kryptoteket.Bot.Models
{
    public class Ticker
    {
        [JsonPropertyName("bid")]
        public string Bid { get; set; }
        [JsonPropertyName("ask")]
        public string Ask { get; set; }
        [JsonPropertyName("spread")]
        public string Spread { get; set; }

    }
}

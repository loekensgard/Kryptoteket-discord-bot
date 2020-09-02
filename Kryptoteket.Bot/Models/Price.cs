using System.Text.Json.Serialization;

namespace Kryptoteket.Bot.Models
{
    public class Price
    {
        [JsonPropertyName("last")]
        public string Last { get; set; }
        [JsonPropertyName("high")]
        public string High { get; set; }
        [JsonPropertyName("change")]
        public string Change { get; set; }
        [JsonPropertyName("low")]
        public string Low { get; set; }
        [JsonPropertyName("ath")]
        public string ATH { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace Kryptoteket.Bot.Models
{
    public class QuickchartUrl
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}

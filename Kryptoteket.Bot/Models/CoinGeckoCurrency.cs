using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Kryptoteket.Bot.Models
{
    public class CoinGeckoCurrency
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}

using System;
using System.Text.Json.Serialization;

namespace Kryptoteket.Bot.Models
{
    public class BitmyntTicker
    {
        [JsonPropertyName("nok")]
        public Nok Nok { get; set; }
    }

    public partial class Nok
    {
        [JsonPropertyName("sell")]
        public string Sell { get; set; }

        [JsonPropertyName("buy")]
        public string Buy { get; set; }
    }
}

using System;
using System.Text.Json.Serialization;

namespace Kryptoteket.Bot.Models
{
    public class NBXOrders
    {
        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("side")]
        public string Side { get; set; }

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("quantity")]
        public string Quantity { get; set; }
    }
}

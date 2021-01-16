using System;
using System.Text.Json.Serialization;

namespace Kryptoteket.Bot.Models
{
    public partial class NBXTrades
    {
        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("takerOrder")]
        public TradeOrder TakerOrder { get; set; }

        [JsonPropertyName("makerOrder")]
        public TradeOrder MakerOrder { get; set; }

        [JsonPropertyName("quantity")]
        public string Quantity { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }
    }

    public partial class TradeOrder
    {
        [JsonPropertyName("account")]
        public Account Account { get; set; }

        [JsonPropertyName("side")]
        public string Side { get; set; }

        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }

    public partial class Account
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }
}

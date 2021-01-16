using System;

namespace Kryptoteket.Bot.Models
{
    public class PlacedBet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int BetId { get; set; }
        public ulong BetUserId { get; set; }
        public DateTimeOffset? BetPlaced { get; set; }
    }
}

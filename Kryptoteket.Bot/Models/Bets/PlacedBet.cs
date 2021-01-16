using System;

namespace Kryptoteket.Bot.Models
{
    public class PlacedBet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public int BetId { get; set; }
        public DateTimeOffset? BetPlaced { get; set; }
    }
}

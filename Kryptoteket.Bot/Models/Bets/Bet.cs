using System;
using System.Collections.Generic;

namespace Kryptoteket.Bot.Models.Bets
{
    public class Bet
    {
        public int BetId { get; set; }
        public string ShortName { get; set; }
        public string AddedBy { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset Locked { get; set; }
        public DateTimeOffset Created { get; set; }
        public ICollection<PlacedBet> PlacedBets { get; set; }
    }
}

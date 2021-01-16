using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Models.Bets
{
    public class BetUser
    {
        public ulong BetUserId { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public ICollection<FinishedBetPlacement> Placements { get; set; }
        public ICollection<PlacedBet> PlacedBets { get; set; }
    }
}

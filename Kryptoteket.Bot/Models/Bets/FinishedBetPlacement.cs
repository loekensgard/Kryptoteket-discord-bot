using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Models.Bets
{
    public class FinishedBetPlacement
    {
        public int Id { get; set; }
        public int BetId { get; set; }
        public string Place { get; set; }
        public ulong BetUserId { get; set; }
    }
}

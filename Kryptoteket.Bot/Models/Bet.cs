using System;
using System.Collections.Generic;

namespace Kryptoteket.Bot.Models
{
    public class Bet
    {
        public string id { get; set; }
        public string ShortName { get; set; }
        public string AddedBy { get; set; }
        public DateTimeOffset Date { get; set; }
        public List<UserBet> Users { get; set; }
    }
}

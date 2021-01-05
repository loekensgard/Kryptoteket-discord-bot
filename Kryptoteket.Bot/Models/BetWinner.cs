using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Models
{
    public class BetWinner
    {
        public string id { get; set; }
        public int Points { get; set; }
        public string Name { get; set; }
        public string[] BetsWon { get; set; }
    }
}

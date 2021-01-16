using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Models.Reflinks
{
    public class RefExchangeRefuser
    {
        public int RefExchangeId { get; set; }
        public RefExchange RefExchange { get; set; }
        public ulong RefUserId { get; set; }
        public RefUser RefUser { get; set; }
    }
}

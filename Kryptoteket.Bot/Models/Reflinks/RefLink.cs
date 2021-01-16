using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Models.Reflinks
{
    public class RefLink
    {
        public int Id { get; set; }
        public int RefExchangeId { get; set; }
        public ulong RefUserId { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
    }
}

using System.Collections.Generic;

namespace Kryptoteket.Bot.Models.Reflinks
{
    public class RefUser
    {
        public ulong RefUserId { get; set; }
        public string Name { get; set; }
        public bool Approved { get; set; }
        public ICollection<RefExchange> RefExchanges { get; set; }
        public ICollection<RefLink> Reflinks { get; set; }
    }
}

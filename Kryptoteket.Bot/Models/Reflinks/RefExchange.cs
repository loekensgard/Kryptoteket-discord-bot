using System.Collections.Generic;

namespace Kryptoteket.Bot.Models.Reflinks
{
    public class RefExchange
    {
        public int RefExchangeId { get; set; }
        public string Name { get; set; }
        public ulong EmojiId { get; set; }
        public ICollection<RefUser> RefUsers { get; set; }
        public ICollection<RefLink> Reflinks { get; set; }
    }
}

using Kryptoteket.Bot.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface IReflinkRepository
    {
        Task AddReflink(ulong id, string name, string reflink, ulong guildId);
        Task<bool> Exists(ulong id, ulong guildId);
        Task<List<Reflink>> GetReflinks(ulong guildId);
        Task<Reflink> GetReflink(ulong id, ulong guildId);
        Task UpdateReflink(ulong id, string reflink, ulong guildId);
        Task DeleteReflink(ulong id, ulong guildId);
    }
}

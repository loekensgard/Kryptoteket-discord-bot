using Kryptoteket.Bot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface IReflinkRepository
    {
        Task AddReflink(ulong id, string name, string reflink);
        Task<bool> Exists(ulong id);
        Task<List<Reflink>> GetReflinks();
        Task<Reflink> GetReflink(ulong id);
        Task UpdateReflink(ulong id, string reflink);
        Task DeleteReflink(ulong id);
    }
}

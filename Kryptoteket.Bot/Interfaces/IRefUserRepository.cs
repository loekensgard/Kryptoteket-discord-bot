using Kryptoteket.Bot.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface IRefUserRepository
    {
        Task CreateRefUser(string id, string name);
        Task<bool> Exists(string id);
        Task<RefUser> GetRefUser(string id);
        Task Update(string id, RefUser reflink);
        Task<List<RefUser>> GetRefUsers(string exchange = null);
    }
}

using Kryptoteket.Bot.Models.Reflinks;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface IRefUserRepository
    {
        Task<RefUser> GetRefUser(ulong id);
        Task<RefUser> CreateRefUser(RefUser refUser);
        Task UpdateUser(RefUser refuser);
    }
}

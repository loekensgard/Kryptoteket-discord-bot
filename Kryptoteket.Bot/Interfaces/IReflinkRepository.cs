using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface IReflinkRepository
    {
        Task<bool> AddReflink(string name, string reflink);
        Task<List<string>> GetReflinks();
    }
}

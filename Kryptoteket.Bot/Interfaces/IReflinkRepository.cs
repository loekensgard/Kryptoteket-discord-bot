using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface IReflinkRepository
    {
        Task AddReflink(string reflink);
        Task<List<string>> GetReflinks();
    }
}

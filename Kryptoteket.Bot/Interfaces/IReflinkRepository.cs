using Kryptoteket.Bot.Models.Reflinks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface IReflinkRepository
    {
        Task<bool> Exist(ulong? id = null, int? refExchangeId = null, string reflink = null);
        Task CreateReflink(RefLink refLink);
        Task UpdateReflink(RefLink link);
        Task DeleteReflink(int id);
    }
}

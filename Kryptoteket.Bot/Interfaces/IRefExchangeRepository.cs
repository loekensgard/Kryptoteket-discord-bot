using Kryptoteket.Bot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface IRefExchangeRepository
    {
        Task<bool> Exists(string exchange);
        Task CreateReflink(string refId, string exchange, string reflink, string userId);
        Task<List<RefExchange>> GetRefExchanges(string userId = null, string exchange = null);
        Task<RefExchange> GetRefExchange(string id);
        Task UpdateRefExchanges(string id, RefExchange reflink);
        Task DeleteReflink(string id);
    }
}

using Kryptoteket.Bot.Models;
using Kryptoteket.Bot.Models.Reflinks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface IRefExchangeRepository
    {
        Task<RefExchange> GetRefExchange(string exchange);
        Task<bool> Exists(string exchange);
        Task CreateRefExchange(RefExchange refExchange);
        Task<List<RefExchange>> GetRefExchanges(string exchange = null);
        Task<RefExchange> UpdateExchange(RefExchange refexch);
    }
}

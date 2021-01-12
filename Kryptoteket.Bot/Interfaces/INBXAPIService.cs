using Kryptoteket.Bot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface INBXAPIService
    {
        Task<Price> GetPrice(string pair);
        Task<Ticker> GetTicker(string pair);
    }
}

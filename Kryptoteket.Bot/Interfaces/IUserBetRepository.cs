using Kryptoteket.Bot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Interfaces
{
    public interface IUserBetRepository
    {
        Task AddUserBet(UserBet userBet);
        Task<List<UserBet>> GetUserBets(string id);
    }
}

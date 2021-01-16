using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models.Reflinks;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.CosmosDB.Repositories
{
    public class RefUserRepository : IRefUserRepository
    {
        private readonly KryptoteketContext _context;
        private readonly DbSet<RefUser> _set;

        public RefUserRepository(KryptoteketContext context)
        {
            _context = context;
            _set = _context.RefUsers;
        }

        public async Task<RefUser> CreateRefUser(RefUser refUser)
        {
            if(refUser != null)
            {
                _set.Add(refUser);
                await _context.SaveChangesAsync();
            }

            return refUser;
        }

        public async Task<RefUser> GetRefUser(ulong id)
        {
            return await _set.Include(x => x.Reflinks).FirstOrDefaultAsync(x => x.RefUserId == id);
        }

        public async Task UpdateUser(RefUser refuser)
        {
            var entity = await _set.FindAsync(refuser.RefUserId);

            if (entity != null)
            {
                _context.Entry(entity).CurrentValues.SetValues(refuser);
                await _context.SaveChangesAsync();
            }
        }
    }
}

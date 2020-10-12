using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.CosmosDB.Repositories
{
    public class ReflinkRepository : IReflinkRepository
    {
        private readonly RegistryContext _context;
        private readonly DbSet<Reflink> _set;

        public ReflinkRepository(RegistryContext context)
        {
            _context = context;
            _set = _context.Reflinks;

        }

        public async Task AddReflink(ulong id, string name, string reflink)
        {
            _set.Add(new Reflink { id = id.ToString(), Name = name, Link = reflink, Approved = false });

            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(ulong id)
        {
            var entity = await _set.FindAsync(id.ToString());
            return entity != null;
        }

        public async Task<List<Reflink>> GetReflinks()
        {
            return await _set.ToListAsync();
        }

        public async Task<Reflink> GetReflink(ulong id)
        {
            return await _set.FindAsync(id.ToString());
        }

        public async Task UpdateReflink(ulong id, string reflink)
        {
            var entity = await _set.FindAsync(id.ToString());
            if (entity != null)
            {
                if (entity.Link.ToLower() != reflink.ToLower()) entity.Link = reflink;
                _set.Update(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteReflink(ulong id)
        {
            var entity = await _set.FindAsync(id.ToString());
            if (entity != null)
            {
                _set.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}

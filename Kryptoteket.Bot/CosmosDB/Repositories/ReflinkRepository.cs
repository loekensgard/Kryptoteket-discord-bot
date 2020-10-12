using Discord.Rest;
using Kryptoteket.Bot.Configurations;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
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
           // await _context.Database.EnsureCreatedAsync();

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

    }
}

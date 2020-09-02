﻿using Kryptoteket.Bot.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Services
{
    public class InitMemoryDB
    {
        private readonly ICoinGeckoAPIService _coinGeckoAPIService;
        private readonly ICoinGeckoRepository _coinGeckoRepository;

        public InitMemoryDB(ICoinGeckoAPIService coinGeckoAPIService, ICoinGeckoRepository coinGeckoRepository)
        {
            _coinGeckoAPIService = coinGeckoAPIService;
            _coinGeckoRepository = coinGeckoRepository;
        }

        public async Task InitDB()
        {
            var list = await _coinGeckoAPIService.GetCoinsList();
            await _coinGeckoRepository.AddCurrency(list);
        }
    }
}

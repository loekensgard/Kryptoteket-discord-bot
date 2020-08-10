using Kryptoteket.Bot.Configurations;
using Kryptoteket.Bot.Exceptions;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Services.API
{
    public class CoinGeckoAPIService : ICoinGeckoAPIService
    {
        private readonly HttpResponseService _httpResponseService;
        private readonly CoinGeckoConfiguration _coinGeckoOptions;

        public CoinGeckoAPIService(HttpResponseService httpResponseService, IOptions<CoinGeckoConfiguration> options)
        {
            _httpResponseService = httpResponseService;
            _coinGeckoOptions = options.Value;
        }

        public async Task<List<Gainers>> GetTopGainers(int top, string timePeriod)
        {
            var resultList = new List<Gainers>();
            int page = ((top - 1) / 250);
            int per_page = top > 250 ? 250 : top;
            int last_page =  top - (page * 250);

            int j = page + 1;
            for(int i = 1; i <= j; i++)
            {
                using (var client = new HttpClient())
                using (var requets = new HttpRequestMessage(HttpMethod.Get, $"{_coinGeckoOptions.APIUri}coins/markets?vs_currency=nok&order=market_cap_desc&per_page={((i == j) ? last_page : per_page)}&page={i}&sparkline=false&price_change_percentage={timePeriod}"))
                {
                    using (var response = await client.SendAsync(requets, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (response.IsSuccessStatusCode)
                            resultList.AddRange(await _httpResponseService.DeserializeJsonFromStream<List<Gainers>>(response));
                    }
                }
            }

            return resultList;
        }
    }
}

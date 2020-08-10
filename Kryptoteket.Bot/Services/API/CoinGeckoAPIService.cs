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
            using (var client = new HttpClient())
            using (var requets = new HttpRequestMessage(HttpMethod.Get, $"{_coinGeckoOptions.APIUri}coins/markets?vs_currency=nok&order=market_cap_desc&per_page={top}&page=1&sparkline=false&price_change_percentage={timePeriod}"))
            {
                using (var response = await client.SendAsync(requets, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                        return await _httpResponseService.DeserializeJsonFromStream<List<Gainers>>(response);

                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound || response.StatusCode == System.Net.HttpStatusCode.NotAcceptable)
                        return null;

                    var content = await _httpResponseService.StreamToStringAsync(await response.Content.ReadAsStreamAsync());

                    throw new ApiException(message: content)
                    {
                        StatusCode = (int)response.StatusCode,
                        Content = content
                    };
                }
            }
        }
    }
}

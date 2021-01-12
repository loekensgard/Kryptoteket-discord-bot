using Kryptoteket.Bot.Configurations;
using Kryptoteket.Bot.Exceptions;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Services.API
{
    public class BitmyntAPIService : IBitmyntAPIService
    {
        private readonly ExchangesConfiguration _exchnagesConfiguration;
        private readonly HttpResponseService _httpResponseService;

        public BitmyntAPIService(IOptions<ExchangesConfiguration> exchnagesConfiguration, HttpResponseService httpResponseService)
        {
            _exchnagesConfiguration = exchnagesConfiguration.Value;
            _httpResponseService = httpResponseService;
        }

        public async Task<Ticker> GetTicker(string pair)
        {
            if (pair.ToLower() != "btcnok") return null;

            var ticker = new BitmyntTicker();
            using (var client = new HttpClient())
            using (var requets = new HttpRequestMessage(HttpMethod.Get, $"{_exchnagesConfiguration.BitmyntAPIUri}ticker-nok.pl"))
            {
                using (var response = await client.SendAsync(requets, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                        ticker = await _httpResponseService.DeserializeJsonFromStream<BitmyntTicker>(response);

                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound || response.StatusCode == System.Net.HttpStatusCode.NotAcceptable)
                        return null;

                    if (!response.IsSuccessStatusCode)
                    {
                        var content = await _httpResponseService.StreamToStringAsync(await response.Content.ReadAsStreamAsync());

                        throw new ApiException(message: content)
                        {
                            StatusCode = (int)response.StatusCode,
                            Content = content
                        };
                    }
                }
            }

            var spread = double.Parse(ticker.Nok?.Sell) - double.Parse(ticker.Nok?.Buy);
            return new Ticker
            {
                Ask = ticker.Nok?.Sell,
                Bid = ticker.Nok?.Buy,
                Spread = spread.ToString("#.##")
            };
        }

    }
}

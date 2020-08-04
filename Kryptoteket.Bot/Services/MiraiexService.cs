using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Services
{
    public class MiraiexService : IMiraiexService
    {
        private readonly HttpResponseService _httpResponseService;

        public MiraiexService(HttpResponseService httpResponseService)
        {
            _httpResponseService = httpResponseService;
        }

        public async Task<Price> GetPrice(string pair)
        {
            using (var client = new HttpClient())
            using (var requets = new HttpRequestMessage(HttpMethod.Get, $"https://api.miraiex.com/v2/markets/{pair.ToUpper()}"))
            {
                using (var response = await client.SendAsync(requets, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                        return await _httpResponseService.DeserializeJsonFromStream<Price>(response);

                    //AddErrorHandling
                    throw new Exception("failed");
                }
            }
        }

        public async Task<Ticker> GetTicker(string pair)
        {
            using (var client = new HttpClient())
            using (var requets = new HttpRequestMessage(HttpMethod.Get, $"https://api.miraiex.com/v2/markets/{pair.ToUpper()}/ticker"))
            {
                using (var response = await client.SendAsync(requets, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                        return await _httpResponseService.DeserializeJsonFromStream<Ticker>(response);

                    //AddErrorHandling
                    throw new Exception("failed");
                }
            }
        }
    }
}

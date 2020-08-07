using Kryptoteket.Bot.Configurations;
using Kryptoteket.Bot.Exceptions;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Services
{
    public class MiraiexService : IMiraiexService
    {
        private readonly ExchangesConfiguration _exchnagesConfiguration;
        private readonly HttpResponseService _httpResponseService;

        public MiraiexService(IOptions<ExchangesConfiguration> exchnagesConfiguration, HttpResponseService httpResponseService)
        {
            _exchnagesConfiguration = exchnagesConfiguration.Value;
            _httpResponseService = httpResponseService;

            if(string.IsNullOrEmpty(_exchnagesConfiguration.MiraiexAPIUri)) throw new ArgumentNullException(nameof(_exchnagesConfiguration.MiraiexAPIUri));
        }

        public async Task<Price> GetPrice(string pair)
        {
            using (var client = new HttpClient())
            using (var requets = new HttpRequestMessage(HttpMethod.Get, $"{_exchnagesConfiguration.MiraiexAPIUri}markets/{pair.ToUpper()}"))
            {
                using (var response = await client.SendAsync(requets, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                        return await _httpResponseService.DeserializeJsonFromStream<Price>(response);

                    if(response.StatusCode == System.Net.HttpStatusCode.NotFound || response.StatusCode == System.Net.HttpStatusCode.NotAcceptable)
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

        public async Task<Ticker> GetTicker(string pair)
        {
            using (var client = new HttpClient())
            using (var requets = new HttpRequestMessage(HttpMethod.Get, $"{_exchnagesConfiguration.MiraiexAPIUri}markets/{pair.ToUpper()}/ticker"))
            {
                using (var response = await client.SendAsync(requets, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                        return await _httpResponseService.DeserializeJsonFromStream<Ticker>(response);

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

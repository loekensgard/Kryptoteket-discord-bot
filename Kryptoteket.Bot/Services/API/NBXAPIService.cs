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

    public class NBXAPIService : INBXAPIService
    {
        private readonly ExchangesConfiguration _exchnagesConfiguration;
        private readonly HttpResponseService _httpResponseService;

        public NBXAPIService(IOptions<ExchangesConfiguration> exchnagesConfiguration, HttpResponseService httpResponseService)
        {
            _exchnagesConfiguration = exchnagesConfiguration.Value;
            _httpResponseService = httpResponseService;
        }

        public async Task<Price> GetPrice(string pair)
        {

            var trades = new List<NBXTrades>();
            var output = new List<NBXTrades>();
            string header;

            try
            {
                //WTF IS THIS
                (output, header) = await CallAPI(pair);
                trades.AddRange(output);
                if (header != null)
                {
                    (output, header) = await CallAPI(pair, header);
                    trades.AddRange(output);
                }
                if(header != null)
                {
                    (output, header) = await CallAPI(pair, header);
                    trades.AddRange(output);
                }
                //END
            }
            catch(Exception)
            {
                throw;
            }


            //Get only last 24h
            var now = DateTime.Now;
            trades.RemoveAll(x => x.CreatedAt < now.AddHours(-24));

            if (trades.Count == 0) throw new NBXTradesNullException();

            try
            {
                var lastBuy = trades[0];
                var low = trades.OrderBy(x => double.Parse(x.Price)).FirstOrDefault();
                var high = trades.OrderByDescending(x => double.Parse(x.Price)).FirstOrDefault();

                var first = trades.OrderBy(x => x.CreatedAt).First();
                var last = trades.OrderByDescending(x => x.CreatedAt).First();
                var change24 = ((double.Parse(last.Price) - double.Parse(first.Price)) / double.Parse(first.Price)) * 100;

                return new Price
                {
                    High = high.Price,
                    Last = lastBuy.Price,
                    Low = low.Price,
                    Change = change24.ToString()
                };
            }
            catch(Exception)
            {
                throw;
            }
        }

        private async Task<(List<NBXTrades>, string)> CallAPI(string pair, string pagination = null)
        {
            using (var client = new HttpClient())
            using (var requets = new HttpRequestMessage(HttpMethod.Get, $"{_exchnagesConfiguration.NBXAPIUri}markets/{pair.Insert(3, "-")}/trades"))
            {
                if (pagination != null) requets.Headers.Add("x-paging-state", pagination);

                using (var response = await client.SendAsync(requets, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var exists = response.Headers.TryGetValues("x-paging-state", out IEnumerable<string> header);
                        return (await _httpResponseService.DeserializeJsonFromStream<List<NBXTrades>>(response), exists ? header.FirstOrDefault() : null);
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound || response.StatusCode == System.Net.HttpStatusCode.NotAcceptable)
                        return (null, null);

                    var content = await _httpResponseService.StreamToStringAsync(await response.Content.ReadAsStreamAsync());

                    throw new ApiException(message: content)
                    {
                        StatusCode = (int)response.StatusCode,
                        Content = content
                    };
                }
            }
        }

        private static string DoubleToPercentageString(double d)
        {
            return "%" + (Math.Round(d, 2) * 100).ToString();
        }
    }
}

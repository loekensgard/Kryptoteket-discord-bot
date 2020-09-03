using Kryptoteket.Bot.Configurations;
using Kryptoteket.Bot.Exceptions;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using Kryptoteket.Bot.Modules;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Services.API
{
    public class CoinGeckoAPIService : ICoinGeckoAPIService
    {
        private readonly HttpResponseService _httpResponseService;
        private readonly ICoinGeckoRepository _coinGeckoRepository;
        private readonly CoinGeckoConfiguration _coinGeckoOptions;

        public CoinGeckoAPIService(HttpResponseService httpResponseService, IOptions<CoinGeckoConfiguration> options, ICoinGeckoRepository coinGeckoRepository)
        {
            _httpResponseService = httpResponseService;
            _coinGeckoRepository = coinGeckoRepository;
            _coinGeckoOptions = options.Value;
        }

        public async Task<List<CoinGeckoCurrency>> GetCoinsList()
        {
            using (var client = new HttpClient())
            using (var requets = new HttpRequestMessage(HttpMethod.Get, $"{_coinGeckoOptions.APIUri}coins/list"))
            {
                using (var response = await client.SendAsync(requets, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                        return await _httpResponseService.DeserializeJsonFromStream<List<CoinGeckoCurrency>>(response);

                    var content = await _httpResponseService.StreamToStringAsync(await response.Content.ReadAsStreamAsync());

                    throw new ApiException(message: content)
                    {
                        StatusCode = (int)response.StatusCode,
                        Content = content
                    };
                }
            }
        }

        public async Task<List<string>> GetSupportedVsCurrenciesList()
        {
            using (var client = new HttpClient())
            using (var requets = new HttpRequestMessage(HttpMethod.Get, $"{_coinGeckoOptions.APIUri}simple/supported_vs_currencies"))
            {
                using (var response = await client.SendAsync(requets, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                        return await _httpResponseService.DeserializeJsonFromStream<List<string>>(response);

                    var content = await _httpResponseService.StreamToStringAsync(await response.Content.ReadAsStreamAsync());

                    throw new ApiException(message: content)
                    {
                        StatusCode = (int)response.StatusCode,
                        Content = content
                    };

                }
            }
        }

        public async Task<Price> GetPrice(string pair)
        {
            var supportedCur = await GetSupprtedCurrency(pair);
            if (string.IsNullOrEmpty(supportedCur)) return null;

            var first = pair.Substring(0, pair.Length - supportedCur.Length);
            var currency = await _coinGeckoRepository.GetCurrency(first);

            if (currency == null) return null;

            var oPrice = new List<CoinGeckoMarketCurrency>();
            using (var client = new HttpClient())
            using (var requets = new HttpRequestMessage(HttpMethod.Get, $"{_coinGeckoOptions.APIUri}coins/markets?ids={currency.Id}&vs_currency={supportedCur}"))
            {
                using (var response = await client.SendAsync(requets, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        oPrice = await _httpResponseService.DeserializeJsonFromStream<List<CoinGeckoMarketCurrency>>(response);
                    }
                    else
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

            var price = new Price()
            {
                Change = oPrice.FirstOrDefault().PriceChangePercentage24H.ToString(),
                High = oPrice.FirstOrDefault().High24H.ToString(),
                Last = oPrice.FirstOrDefault().CurrentPrice.ToString(),
                Low = oPrice.FirstOrDefault().Low24H.ToString(),
                ATH = oPrice.FirstOrDefault().Ath.ToString()
            };

            return price;
        }

        private async Task<string> GetSupprtedCurrency(string pair)
        {
            var supportedCur = await GetSupportedVsCurrenciesList();
            var last3 = pair.Substring(pair.Length - 3);
            var last4 = pair.Substring(pair.Length - 4);

            var last = supportedCur.FirstOrDefault(s => s == last3.ToLower());
            if (last == null) last = supportedCur.FirstOrDefault(s => s == last4.ToLower());
            if (last == null) return null;

            return last;
        }

        public async Task<List<Gainers>> GetTopGainers(int top, string timePeriod)
        {
            var resultList = new List<Gainers>();
            int page = ((top - 1) / 250);
            int per_page = top > 250 ? 250 : top;
            int last_page = top - (page * 250);

            int j = page + 1;
            for (int i = 1; i <= j; i++)
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

        public async Task<ChartResult> Get7dChart(string currency)
        {
            var coin = await _coinGeckoRepository.GetCurrency(currency);
            if (coin == null) return null;

            var sparklines = new List<CoingGeckoSparkline>();
            using (var client = new HttpClient())
            using (var requets = new HttpRequestMessage(HttpMethod.Get, $"{_coinGeckoOptions.APIUri}coins/markets?vs_currency=usd&ids={coin.Id}&order=market_cap_desc&per_page=100&page=1&sparkline=true&price_change_percentage=7d"))
            {
                using (var response = await client.SendAsync(requets, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        sparklines = await _httpResponseService.DeserializeJsonFromStream<List<CoingGeckoSparkline>>(response);
                    }
                    else
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

            var sparkline = sparklines.FirstOrDefault();
            if (sparkline == null) return null;

            var chart = GetChartData(coin, sparkline);
            if(chart == null) return null;

            var uri = new UriBuilder("https://quickchart.io")
            {
                Port = -1,
                Path = "chart",
                Query = $"?c={WebUtility.UrlEncode(JsonSerializer.Serialize(chart))}"
            };

            return new ChartResult { Name = coin.Name, Uri = uri.ToString() };
        }

        private Chart GetChartData(CoinGeckoCurrency coin, CoingGeckoSparkline sparkline)
        {
            var prices = sparkline.SparklineIn7D.Price;

            var max = prices.Max() * 1.01;
            var min = prices.Min() * 0.95; 

            if(!sparkline.SparklineIn7D.Price.Any()) return null;

            var data = new List<double>();
            foreach(var price in sparkline.SparklineIn7D.Price)
            {
                if (price > 0 && price < 1)
                {
                    max = Math.Truncate(max * 1000) / 1000;
                    max = Math.Truncate(min * 1000) / 1000;
                    data.Add(Math.Truncate(price * 1000 ) / 1000);
                }
                else
                {
                    max = Math.Truncate(max);
                    max = Math.Truncate(min);
                    data.Add(Math.Truncate(price * 100 ) / 100);
                }
            }

            var dataset = new Dataset
            {
                BorderColor = sparkline.PriceChangePercentage7d > 0 ? "green" : "red",
                Fill = false,
                Label = coin.Name,
                Data = data
            };

            var options = new Models.Options
            {
                Scales = new Scales
                {
                    YAxes = new List<YAx>
                    {
                        new YAx
                        {
                            Ticks = new Ticks
                            {
                                SuggestedMax = max,
                                SuggestedMin = min
                            }
                        }
                    }
                }
            };

            var chart = new Chart
            {
                Type = "sparkline",
                Data = new Data
                {
                    Datasets = new List<Dataset>
                    {
                        dataset
                    }
                },
                Options = options
            };

            return chart;
        }
    }
}

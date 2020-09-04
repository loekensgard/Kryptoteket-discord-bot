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
using System.Text.Json;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Services.API
{
    public class QuickchartAPIService : IQuickchartAPIService
    {
        private readonly QuickchartConfiguration _quickchartOptions;
        private readonly HttpResponseService _httpResponseService;

        public QuickchartAPIService(IOptions<QuickchartConfiguration> quickchartOptions, HttpResponseService httpResponseService)
        {
            _quickchartOptions = quickchartOptions.Value;
            _httpResponseService = httpResponseService;
        }

        public async Task<string> GetQuickchartURI(CoinGeckoCurrency coin, CoingGeckoSparkline sparkline)
        {
            var chartData = GetChartData(coin, sparkline);
            if (chartData == null) return null;

            var quickchartresult = new QuickchartUrl();
            using (var client = new HttpClient())
            using (var requets = new HttpRequestMessage(HttpMethod.Post, $"{_quickchartOptions.Uri}chart/create"))
            {
                var json = JsonSerializer.Serialize(chartData);
                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    requets.Content = stringContent;
                    using (var response = await client.SendAsync(requets, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            quickchartresult = await _httpResponseService.DeserializeJsonFromStream<QuickchartUrl>(response);
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
            }

            return quickchartresult.Success ? quickchartresult.Url : null;
        }

        private ChartBuilder GetChartData(CoinGeckoCurrency coin, CoingGeckoSparkline sparkline)
        {
            var prices = sparkline.SparklineIn7D.Price;
            if (!sparkline.SparklineIn7D.Price.Any()) return null;

            var data = FillData(prices);

            var dataset = new Dataset
            {
                BorderColor = sparkline.PriceChangePercentage7d > 0 ? "green" : "red",
                Fill = false,
                Label = coin.Name,
                Data = data.Data
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
                                SuggestedMax = data.Max,
                                SuggestedMin = data.Min
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

            return new ChartBuilder {
                Chart = chart
            };
        }

        private static ChartData FillData(List<double> prices)
        {
            var max = prices.Max() * 1.01;
            var min = prices.Min() * 0.95;

            var data = new List<double>();
            foreach (var price in prices)
            {
                if (price > 0 && price < 1)
                {
                    max = Math.Truncate(max * 1000) / 1000;
                    max = Math.Truncate(min * 1000) / 1000;
                    data.Add(Math.Truncate(price * 1000) / 1000);
                }
                else if (price > 10000 && price < 100000)
                {
                    max = Math.Truncate(max);
                    max = Math.Truncate(min);
                    data.Add(Math.Truncate(price));
                }
                else
                {
                    max = Math.Truncate(max);
                    max = Math.Truncate(min);
                    data.Add(Math.Truncate(price * 100) / 100);
                }
            }

            return new ChartData
            {
                Data = data,
                Max = max,
                Min = min
            };
        }
    }
}

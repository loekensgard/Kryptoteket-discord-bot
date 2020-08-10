using Discord;
using Kryptoteket.Bot.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Kryptoteket.Bot.Services
{
    public class EmbedService
    {
        public EmbedBuilder EmbedTicker(string pair, Ticker ticker, string exchangeName)
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle($"{pair.ToUpper()} Ticker - {exchangeName}");
            builder.AddField("Ask", RemoveDecimals(ticker.Ask));
            builder.AddField("Bid", RemoveDecimals(ticker.Bid));
            builder.AddField("Spread", RemoveDecimals(ticker.Spread));
            builder.WithColor(Color.DarkBlue);
            return builder;
        }

        public EmbedBuilder EmbedPrice(string pair, Price price, string exchangeName)
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle($"{pair.ToUpper()} Price - {exchangeName}");
            builder.AddField("Last", price.Last);
            builder.AddField("High", price.High);
            builder.AddField("Low", price.Low);
            builder.AddField("Change", $"{price.Change}%");
            builder.WithColor(Color.DarkBlue);
            return builder;
        }

        public EmbedBuilder EmbedSupportMe()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("Support my bot");
            builder.WithDescription("Thank you for using my bot. If it gives you value, please consider helping me out with the cost by using any of the listed methods.");
            builder.WithColor(Color.DarkBlue);
            builder.AddField("Referral links", $"[MiraiEx](https://miraiex.com/affiliate/?referral=thorshi){Environment.NewLine}[Bittrex](https://bittrex.com/Account/Register?referralCode=L2O-PNM-LLA){Environment.NewLine}[Binance](https://www.binance.com/en/register?ref=P7BWV9S0){Environment.NewLine}[Coinbase](https://www.coinbase.com/join/lkensg_g)");
            builder.AddField("Donate", "*Coming soon*");
            builder.WithFooter(footer => footer.Text = $"Best Regards{Environment.NewLine}Thorshi#6851");
            return builder;
        }

        public EmbedBuilder EmbedTopGainers(List<Gainers> topGainers, int top, string timePeriod)
        {
            EmbedBuilder builder = new EmbedBuilder();
            StringBuilder sb = new StringBuilder();

            builder.WithTitle($"Top gains of top {top} coins last {timePeriod}");
            foreach (var gainer in topGainers.OrderByDescending(o => o.PriceChangeInPeriod).Take(20))
            {
                sb.AppendLine($"**{gainer.Symbol.ToUpper()}:** {Math.Truncate((double)gainer.PriceChangeInPeriod * 100) / 100}%");
            }

            builder.WithDescription(sb.ToString());
            builder.WithColor(Color.DarkBlue);
            builder.WithFooter(footer => footer.Text = $"Updated: {topGainers.First().LastUpdated.ToString("dd.MM.yy HH:mm")}");

            return builder;
        }

        public EmbedBuilder EmbedCovidStats(string title, long totalCases, long totalNewCasesToday, long totalDeaths, long totalNewDeathsToday, long totalRecovered, long updated, long? totaltCasesYesterday = null)
        {
            EmbedBuilder builder = new EmbedBuilder();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Confirmed Cases: {totalCases}");
            sb.AppendLine($"Confirmed Deaths: {totalDeaths}");
            sb.AppendLine($"Confirmed Recoveries: {totalRecovered}");
            sb.AppendLine($"New Cases Today: {totalNewCasesToday}");
            sb.AppendLine($"New Deaths Today: {totalNewDeathsToday}");
            if (totaltCasesYesterday.HasValue)
                sb.AppendLine($"New Cases Yesterday: {totaltCasesYesterday.Value}");

            builder.WithTitle($"Current Coronavirus Statistics for {title}");
            builder.WithDescription(sb.ToString());
            builder.WithColor(Color.Red);

            //Convert UNIX Epoch to readable string
            var readabletime = DateTimeOffset.FromUnixTimeMilliseconds(updated);

            builder.WithFooter(footer => footer.Text = $"Updated: {readabletime.ToString("dd.MM.yy HH:mm")}");
            return builder;
        }

        public EmbedBuilder Embedhelp()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle($"Commands");
            builder.AddField("Crypto", $"!ticker <pair>{Environment.NewLine}!price <pair>{Environment.NewLine}!gainers <top> <1h / 24h / 7d / 14d / 30d / 200d / 1y>");
            builder.AddField("Covid", $"!covid <countryCode / countryName>");
            builder.AddField("Support me", "!support");
            builder.WithColor(Color.DarkBlue);
            builder.WithFooter(footer => footer.Text = "Bot improvements can be featured to Thorshi#6851");
            return builder;
        }

        private string RemoveDecimals(string number)
        {
            if (string.IsNullOrEmpty(number)) throw new ArgumentException("message", nameof(number));

            var check = decimal.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out var result);

            return !check ? "Error parsing" : result.ToString("G29");
        }
    }
}

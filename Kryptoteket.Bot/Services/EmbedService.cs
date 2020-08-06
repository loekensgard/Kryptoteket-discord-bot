using Discord;
using Kryptoteket.Bot.Models;
using System;
using System.Globalization;
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

        public EmbedBuilder EmbedCovidStats(string title, Uri source, long totalCases, long totalNewCasesToday, long totalDeaths, long totalNewDeathsToday, long totalRecovered)
        {
            EmbedBuilder builder = new EmbedBuilder();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Confirmed Cases: {totalCases}");
            sb.AppendLine($"Confirmed Deaths: {totalDeaths}");
            sb.AppendLine($"Confirmed Recoveries: {totalRecovered}");
            sb.AppendLine($"New Cases Today: {totalNewCasesToday}");
            sb.AppendLine($"New Deaths Today: {totalNewDeathsToday}");

            builder.WithTitle($"Current Coronavirus Statistics for {title}");
            builder.WithDescription(sb.ToString());
            builder.WithColor(Color.Red);
            builder.WithFooter(footer => footer.Text = $"Source: {source.AbsoluteUri}");
            return builder;
        }

        public EmbedBuilder Embedhelp()
        {
            EmbedBuilder builder = new EmbedBuilder();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Search ticker: !ticker <pair>");
            sb.AppendLine("Search price: !price <pair>");
            sb.AppendLine("Covid stats: !covid <countryCode>");
            
            builder.WithTitle($"Commands");
            builder.WithDescription(sb.ToString());
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

using Discord;
using Kryptoteket.Bot.Models;
using System;
using System.Globalization;

namespace Kryptoteket.Bot.Services
{
    public class EmbedService
    {
        public EmbedBuilder EmbedTicker(string pair, Ticker ticker)
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle($"{pair.ToUpper()} Ticker");
            builder.AddField("Ask", RemoveDecimals(ticker.Ask));
            builder.AddField("Bid", RemoveDecimals(ticker.Bid));
            builder.AddField("Spread", RemoveDecimals(ticker.Spread));
            builder.WithTimestamp(DateTime.Now);
            builder.WithColor(Color.DarkBlue);
            return builder;
        }

        public EmbedBuilder EmbedPrice(string pair, Price price)
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle($"{pair.ToUpper()} Price");
            builder.AddField("Last", price.Last);
            builder.AddField("High", price.High);
            builder.AddField("Low", price.Low);
            builder.AddField("Change", $"{price.Change}%");
            builder.WithTimestamp(DateTime.Now);
            builder.WithColor(Color.DarkBlue);
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

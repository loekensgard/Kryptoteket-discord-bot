using Discord;
using Discord.WebSocket;
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
            if (price.ATH != null) builder.AddField("ATH", price.ATH);
            builder.AddField("Change last 24h", $"{Math.Truncate((double)Convert.ToDouble(price.Change, CultureInfo.InvariantCulture) * 100) / 100}%");
            builder.WithColor(Color.DarkBlue);
            return builder;
        }


        public EmbedBuilder EmbedSparkline(ChartResult result)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle($"7 day graph for {result.Name}");
            builder.WithImageUrl(result.Uri);

            return builder;
        }

        public EmbedBuilder EmbedSupportMe()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("Support my bot");
            builder.WithDescription("Thank you for using my bot. If it gives you value, please consider helping me out with the cost by using any of the listed methods.");
            builder.WithColor(Color.DarkBlue);
            builder.AddField("Referral links", $"[MiraiEx](https://miraiex.com/affiliate/?referral=thorshi){Environment.NewLine}[Bittrex](https://bittrex.com/Account/Register?referralCode=L2O-PNM-LLA){Environment.NewLine}[Binance](https://www.binance.com/en/register?ref=P7BWV9S0){Environment.NewLine}[Coinbase](https://www.coinbase.com/join/lkensg_g)");
            builder.AddField("Donate", $"Ethereum: 0x3BEad04d72168bF84786Df0B1d325bF277953a80{Environment.NewLine}Bitcoin: bc1qut7xkpvj6penr8ylu86h5gwhwf57yx9nmgwn83");
            builder.WithFooter(footer => footer.Text = $"Best Regards{Environment.NewLine}Thorshi#6851");
            return builder;
        }

        public EmbedBuilder EmbedTopGainers(List<Gainers> topGainers, int top, string timePeriod)
        {
            EmbedBuilder builder = new EmbedBuilder();
            StringBuilder sb = new StringBuilder();

            builder.WithTitle($"Top gains of top {top} coins last {timePeriod}");
            foreach (var gainer in topGainers.OrderByDescending(o => o.PriceChangeInPeriod).Take(20).Where(o => o.PriceChangeInPeriod.HasValue))
            {
                sb.AppendLine($"**{gainer.Symbol.ToUpper()}:** {Math.Truncate((double)gainer.PriceChangeInPeriod * 100) / 100}%");
            }

            builder.WithDescription(sb.ToString());
            builder.WithColor(Color.DarkBlue);
            builder.WithFooter(footer => footer.Text = $"Updated: {topGainers.First().LastUpdated.ToString("dd.MM.yy HH:mm")}");

            return builder;
        }

        public EmbedBuilder EmbedBets(Bet bet)
        {
            EmbedBuilder builder = new EmbedBuilder();
            StringBuilder sb = new StringBuilder();

            builder.WithTitle($"{bet.Date.ToString("dd/M/yyyy", CultureInfo.GetCultureInfo("nb-NO"))}");
            foreach (var userBet in bet.Users.OrderByDescending(p => int.Parse(p.Price)))
            {
                int price;
                if (int.TryParse(userBet.Price, out price))
                    sb.AppendLine($"**{userBet.Name}:** ${price:#,##0}");
            }

            builder.WithDescription(sb.ToString());
            builder.WithColor(Color.DarkBlue);

            return builder;
        }

        public EmbedBuilder EmbedOwnRef(Reflink reflink)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle(reflink.Name);
            builder.AddField("Approved", reflink.Approved.ToString());
            builder.AddField("Link", reflink.Link);

            if (!reflink.Approved)
            {
                builder.WithFooter(footer => footer.Text = "Mods and admins can approve");
            }

            return builder;
        }

        public EmbedBuilder EmbedTopLosers(List<Gainers> topGainers, int top, string timePeriod)
        {
            EmbedBuilder builder = new EmbedBuilder();
            StringBuilder sb = new StringBuilder();

            builder.WithTitle($"Top losers of top {top} coins last {timePeriod}");
            foreach (var gainer in topGainers.OrderBy(o => o.PriceChangeInPeriod).Take(20).Where(o => o.PriceChangeInPeriod.HasValue))
            {
                sb.AppendLine($"**{gainer.Symbol.ToUpper()}:** {Math.Truncate((double)gainer.PriceChangeInPeriod.Value * 100) / 100}%");
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
            builder.AddField("Crypto", $"!ticker <pair>{Environment.NewLine}!price <pair> <mx / nbx>{Environment.NewLine}!gainers <top> <1h / 24h / 7d / 14d / 30d / 200d / 1y>{Environment.NewLine}!losers <top> <1h / 24h / 7d / 14d / 30d / 200d / 1y>{Environment.NewLine}!graph <currency>");
            builder.AddField("Bet", $"!addbet <name> <date>{Environment.NewLine}!deletebet <name>{Environment.NewLine}!bet <name> <price>{Environment.NewLine}!getbet <name>");
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

        public EmbedBuilder EmbedAllReflinks(List<Reflink> reflinks)
        {
            var content = "";
            foreach (var link in reflinks.OrderByDescending(r => r.Approved))
            {
                var approved = "Not Approved";
                if (link.Approved) approved = "Approved";
                content += $"**{link.Name}**: {link.Link} : **{approved}**{Environment.NewLine}";
            }

            EmbedBuilder builder = new EmbedBuilder();
            builder.AddField("Reflinks", $"{content}");
            builder.WithFooter(footer => footer.Text = "Add your link with !addref <link>");

            return builder;
        }

        public EmbedBuilder EmbedServerInfo(SocketGuild guild)
        {
            var online = guild.Users.Where(x => x.Status == UserStatus.Online);
            var bots = guild.Users.Where(x => x.IsBot);
            var textCh = guild.TextChannels.Count();

            EmbedBuilder builder = new EmbedBuilder();
            builder.WithColor(Color.Gold);
            builder.WithTitle("Server Info");
            //builder.WithImageUrl(guild.IconUrl);
            //builder.WithDescription($"{guild.Name}'s information");
            builder.AddField("Owner", guild.GetUser(guild.OwnerId)?.Username ?? "Casper!", true);
            builder.AddField("Region", guild.VoiceRegionId, true);
            builder.AddField("Channel Categories", guild.CategoryChannels.Count(), true);
            builder.AddField("Text Channels", guild.TextChannels.Count(), true);
            builder.AddField("Voice Channels", guild.VoiceChannels.Count(), true);
            builder.AddField("Roles", guild.Roles.Count, true);
            builder.AddField("Members", guild.MemberCount, true);
            builder.AddField("Online", online.Count(), true);
            builder.AddField("Bots", bots.Count(), true);
            builder.AddField("Created", guild.CreatedAt.ToString("dd.MM.yy"));
            builder.WithThumbnailUrl(guild.IconUrl);
            builder.WithCurrentTimestamp();

            return builder;
        }

        public EmbedBuilder EmbedMyInfo(SocketGuild guild, SocketGuildUser user)
        {
            var sortedJoinedMembers = guild.Users.OrderBy(x => x.JoinedAt).ToList();
            int index = sortedJoinedMembers.FindIndex(x => x.Id == user.Id);
            var roles = user.Roles.Where(x => !x.IsEveryone);

            EmbedBuilder builder = new EmbedBuilder();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Name: **{user.Username}#{user.Discriminator}**");
            sb.Append($"Roles: ");

            if (!roles.Any())
                sb.Append("**None**");
            else
            {
                foreach (var role in roles)
                {
                    var ro = role.Name.Replace("@", String.Empty);
                    sb.Append("**");
                    sb.Append($"{StringExtensions.FirstCharToUpper(ro)} ");
                    sb.Append("**");
                }
            }

            sb.AppendLine();
            sb.AppendLine($"Nickname: **{user.Nickname ?? "None"}**");
            sb.AppendLine($"Account Created: **{user.CreatedAt:dd.MM.yy}**");
            sb.Append($"Server Joined: **{user.JoinedAt?.ToString("dd.MM.yy")}** **`(#{index + 1})`**");
            sb.AppendLine();
            if (user.Username.ToLower() == "bredesen")
                sb.AppendLine("Big PP: **Yes**");

            builder.WithTitle($"Userinfo about {user.Username}");
            builder.WithDescription(sb.ToString());
            builder.WithThumbnailUrl(user.GetAvatarUrl());
            builder.WithColor(Color.Gold);

            return builder;
        }
    }
}

# Kryptoteket-crypto-bot
Discord bot for retrieving crypto prices and tickers at [Kryptotekets discord](https://discord.gg/heWSa5n).

## Getting started

1. Create your own discord bot by following this [guide](https://discordpy.readthedocs.io/en/latest/discord.html).
2. Copy your bot token and past it in [AppSettings](https://github.com/loekensgard/kryptoteket-crypto-bot/blob/master/Kryptoteket.Bot/appsettings.json) under token.
3. Run the project in your favorite .net IDE.

## Discord commands

```
!ticker btcnok
!ticker ethnok

!price btcnok
!price ethnok
```

These can be found under the Modules folder.\
All data is currently retrieved by public APIs at [MiraiEx](https://developers.miraiex.com/).

## Contributing
Pull requests are welcome, please discuss changes via Issues. 

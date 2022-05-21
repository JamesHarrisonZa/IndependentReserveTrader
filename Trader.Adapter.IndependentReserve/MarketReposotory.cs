using IndependentReserve.DotNetClientApi;
using Microsoft.Extensions.Configuration;
using Trader.Domain.Enums;
using Trader.Domain.OutboundPorts;
using Trader.Adapter.IndependentReserve.Config;

namespace Trader.Adapter.IndependentReserve;

public class MarketRepository : IMarketRepository
{
    private readonly Client _client;

    public MarketRepository(IConfiguration configuration)
    {
        var independentReserveConfig = configuration.GetSection("IndependentReserve").Get<IndependentReserveConfig>();

        var apiConfig = new ApiConfig(independentReserveConfig.BaseUrl, independentReserveConfig.ApiKey, independentReserveConfig.ApiSecret);
        _client = Client.Create(apiConfig);
    }

    public async Task<decimal> GetCurrentPrice(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency)
    {
        var currencyCode = CodeConverter.GetCurrencyCode(cryptoCurrency);
        var fiatCurrencyCode = CodeConverter.GetCurrencyCode(fiatCurrency);

        var currencyCodeSummary = await _client.GetMarketSummaryAsync(currencyCode, fiatCurrencyCode); //ToDo: Share or add caching
        
        return currencyCodeSummary?.LastPrice ?? 0;
    }

    public async Task PlaceBuyOrder(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency, decimal fiatAmount)
    {
        var currencyCode = CodeConverter.GetCurrencyCode(cryptoCurrency);
        var fiatCurrencyCode = CodeConverter.GetCurrencyCode(fiatCurrency);

        var cryptoVolume = await GetCryptoAmount(cryptoCurrency, fiatCurrency, fiatAmount);

        Console.WriteLine($"Volume for {fiatAmount} is {cryptoVolume}");

        // var response = await _client.PlaceMarketOrderAsync(currencyCode, fiatCurrencyCode, OrderType.MarketOffer, cryptoVolume);
    }

    private async Task<decimal> GetCryptoAmount(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency, decimal fiatAmount)
    {
        var currentPrice = await GetCurrentPrice(cryptoCurrency, fiatCurrency);

        var cryptoAmount = Math.Round(fiatAmount/currentPrice, 7);

        return cryptoAmount;
    }
}

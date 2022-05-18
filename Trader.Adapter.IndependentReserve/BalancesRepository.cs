using IndependentReserve.DotNetClientApi;
using IndependentReserve.DotNetClientApi.Data;
using Microsoft.Extensions.Configuration;
using Trader.Domain.Enums;
using Trader.Domain.OutboundPorts;
using Trader.Adapter.IndependentReserve.Config;

namespace Trader.Adapter.IndependentReserve;

public class BalancesRepository : IBalancesRepository
{
    private readonly Client _client;

    public BalancesRepository(IConfiguration configuration)
    {
        var independentReserveConfig = configuration.GetSection("IndependentReserve").Get<IndependentReserveConfig>();

        var apiConfig = new ApiConfig(independentReserveConfig.BaseUrl, independentReserveConfig.ApiKey, independentReserveConfig.ApiSecret);
        _client = Client.Create(apiConfig);
    }

    public async Task<decimal> GetBalance(CryptoCurrency cryptoCurrency)
    {
        var currencyCode = GetCurrencyCode(cryptoCurrency);

        var accounts = await _client.GetAccountsAsync(); //ToDo: Share or add caching
        var currencyCodeAccount = accounts.FirstOrDefault(a => a.CurrencyCode == currencyCode);

        return currencyCodeAccount?.TotalBalance ?? 0;
    }

    public async Task<decimal> GetCurrentPrice(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency)
    {
        var currencyCode = GetCurrencyCode(cryptoCurrency);
        var fiatCurrencyCode = GetCurrencyCode(fiatCurrency);

        var currencyCodeSummary = await _client.GetMarketSummaryAsync(currencyCode, fiatCurrencyCode); //ToDo: Share or add caching
        
        return currencyCodeSummary?.LastPrice ?? 0;
    }

    public async Task<decimal> GetBalanceValue(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency)
    {
        var balance = await GetBalance(cryptoCurrency);
        var currentPrice = await GetCurrentPrice(cryptoCurrency, fiatCurrency);

        return Math.Round(balance * currentPrice, 2);
    }

    public CurrencyCode GetCurrencyCode(CryptoCurrency code)
    {
        if (code.ToString() == "BTC")
            return CurrencyCode.Xbt;
        if (code.ToString() == "ETH")
            return CurrencyCode.Eth;

        throw new ArgumentException($"Invalid code: {code}");
    }

    public CurrencyCode GetCurrencyCode(FiatCurrency code)
    {
        if (code.ToString() == "NZD")
            return CurrencyCode.Nzd;
        if (code.ToString() == "USD")
            return CurrencyCode.Usd;

        throw new ArgumentException($"Invalid code: {code}");
    }

}
using Microsoft.Extensions.Configuration;
using IndependentReserve.DotNetClientApi;
using IndependentReserve.DotNetClientApi.Data;
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

    public async Task<decimal> GetBalance(string code)
    {
        var currencyCode = GetCurrencyCode(code);

        var accounts = await _client.GetAccountsAsync(); //ToDo: Share or add caching
        var currencyCodeAccount = accounts.FirstOrDefault(a => a.CurrencyCode == currencyCode);

        return currencyCodeAccount?.TotalBalance ?? 0;
    }

    public async Task<decimal> GetCurrentPrice(string code, string fiatCurrency)
    {
        var currencyCode = GetCurrencyCode(code);
        var fiatCurrencyCode = GetCurrencyCode(fiatCurrency);

        var currencyCodeSummary = await _client.GetMarketSummaryAsync(currencyCode, fiatCurrencyCode); //ToDo: Share or add caching
        
        return currencyCodeSummary?.LastPrice ?? 0;
    }

    public async Task<decimal> GetBalanceValue(string code, string fiatCurrency)
    {
        var balance = await GetBalance(code);
        var currentPrice = await GetCurrentPrice(code, fiatCurrency);

        return Math.Round(balance * currentPrice, 2);
    }

    public CurrencyCode GetCurrencyCode(string code)
    {
        //TODO: Something smarter. Its late
        if (code == "BTC")
            return CurrencyCode.Xbt;
        if (code == "ETH")
            return CurrencyCode.Eth;

        if (code == "NZD")
            return CurrencyCode.Nzd;
        if (code == "USD")
            return CurrencyCode.Usd;

        throw new ArgumentException($"Invalid code: {code}");
    }

}
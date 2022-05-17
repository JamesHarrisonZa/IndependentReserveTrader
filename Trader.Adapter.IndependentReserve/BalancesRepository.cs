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

    public async Task<decimal> GetBitCoinBalance()
    {
        var accounts = await _client.GetAccountsAsync(); //ToDo: Share or add caching
        var btcAccount = accounts.FirstOrDefault(a => a.CurrencyCode == CurrencyCode.Xbt);

        return btcAccount?.TotalBalance ?? 0;
    }

    public async Task<decimal> GetEtheriumBalance()
    {
        var accounts = await _client.GetAccountsAsync(); //ToDo: Share or add caching
        var ethAccount = accounts.FirstOrDefault(a => a.CurrencyCode == CurrencyCode.Eth);

        return ethAccount?.TotalBalance ?? 0;
    }

    public async Task<decimal> GetBitCoinCurrentPrice()
    {
        var btcSummary = await _client.GetMarketSummaryAsync(CurrencyCode.Xbt, CurrencyCode.Nzd); //ToDo: Share or add caching
        
        return btcSummary?.LastPrice ?? 0;
    }

    public async Task<decimal> GetEtheriumCurrentPrice()
    {
        var ethSummary = await _client.GetMarketSummaryAsync(CurrencyCode.Eth, CurrencyCode.Nzd); //ToDo: Share or add caching
        
        return ethSummary?.LastPrice ?? 0;
    }
}
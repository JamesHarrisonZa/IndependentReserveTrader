using IndependentReserve.DotNetClientApi;
using Trader.Domain.Enums;
using Trader.Domain.OutboundPorts;
using Trader.Adapter.IndependentReserve.Config;

namespace Trader.Adapter.IndependentReserve.Repositories;

public class BalancesRepository : IBalancesRepository
{
    private readonly IMarketRepository _marketRepository;
    private readonly Client _client;

    public BalancesRepository(IMarketRepository marketRepository, IndependentReserveConfig independentReserveConfig)
    {
        _marketRepository = marketRepository;

        var apiConfig = new ApiConfig(independentReserveConfig.BaseUrl, independentReserveConfig.ApiKey, independentReserveConfig.ApiSecret);
        _client = Client.Create(apiConfig);
    }

    public async Task<decimal> GetBalance(CryptoCurrency cryptoCurrency)
    {
        var currencyCode = CodeConverter.GetCurrencyCode(cryptoCurrency);

        var accounts = await _client.GetAccountsAsync(); //ToDo: Share or add caching
        var currencyCodeAccount = accounts.FirstOrDefault(a => a.CurrencyCode == currencyCode);

        return currencyCodeAccount?.TotalBalance ?? 0;
    }

    public async Task<decimal> GetBalanceValue(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency)
    {
        var balance = await GetBalance(cryptoCurrency);
        var currentPrice = await _marketRepository.GetCurrentPrice(cryptoCurrency, fiatCurrency);

        return Math.Round(balance * currentPrice, 2);
    }
}
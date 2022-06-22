namespace Trader.Domain.Services;

public class MarketReader : IMarketReader
{
    private readonly IMarketRepository _marketRepository;

    public MarketReader(IMarketRepository balancesRepository)
    {
        _marketRepository = balancesRepository;
    }

    public async Task<decimal> GetBitcoinLastPrice(FiatCurrency fiatCurrency)
    {
        return await _marketRepository
            .GetLastPrice(CryptoCurrency.BTC, fiatCurrency);
    }

    public async Task<decimal> GetEtheriumLastPrice(FiatCurrency fiatCurrency)
    {
        return await _marketRepository
            .GetLastPrice(CryptoCurrency.ETH, fiatCurrency);
    }

    public async Task<ClosedOrder> GetBitcoinLastClosedOrder()
    {
        return await _marketRepository
            .GetLastClosedOrder(CryptoCurrency.BTC, FiatCurrency.NZD);
    }

    public async Task<decimal> GetMarketValueOfClosedOrder(ClosedOrder closedOrder)
    {
        return await GetMarketValue(closedOrder.CryptoCurrency, closedOrder.Volume, closedOrder.FiatCurrency);
    }

    private async Task<decimal> GetMarketValue(CryptoCurrency cryptoCurrency, decimal cryptoAmount, FiatCurrency fiatCurrency)
    {
        var currentPrice = await _marketRepository.GetLastPrice(cryptoCurrency, fiatCurrency);
        var currentValue = Math.Round(cryptoAmount * currentPrice, 2);

        return currentValue;
    }
}
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
            .GetLastMarketPrice(CryptoCurrency.BTC, fiatCurrency);
    }

    public async Task<decimal> GetEtheriumLastPrice(FiatCurrency fiatCurrency)
    {
        return await _marketRepository
            .GetLastMarketPrice(CryptoCurrency.ETH, fiatCurrency);
    }

    public async Task<ClosedOrder> GetBitcoinLastClosedOrder()
    {
        return await _marketRepository
            .GetLastClosedOrder(CryptoCurrency.BTC, FiatCurrency.NZD);
    }

    public async Task<MarketClosedOrder> GetMarketValueOfClosedOrder(ClosedOrder closedOrder)
    {
        var marketValue = await GetMarketValue(closedOrder.CryptoCurrency, closedOrder.Volume, closedOrder.FiatCurrency);
        var isProfitable = CalculateIsProfitable(closedOrder.Value, marketValue);
        var gainOrLossPercentage = CalculateGainOrLossPercentage(closedOrder.Value ?? 0, marketValue);

        return new MarketClosedOrder(closedOrder)
        {
            MarketValue = marketValue,
            IsProfitable = isProfitable,
            GainOrLossPercentage = gainOrLossPercentage,
        };
    }

    private async Task<decimal> GetMarketValue(CryptoCurrency cryptoCurrency, decimal cryptoAmount, FiatCurrency fiatCurrency)
    {
        var currentPrice = await _marketRepository.GetLastMarketPrice(cryptoCurrency, fiatCurrency);
        var currentValue = Math.Round(cryptoAmount * currentPrice, 2);

        return currentValue;
    }

    private bool CalculateIsProfitable(decimal? orderValue, decimal marketValue)
    {
        //TODO factor in fees. Coming soon

        return marketValue > orderValue;
    }

    private decimal CalculateGainOrLossPercentage(decimal orderValue, decimal marketValue)
    {
        var percentage = (marketValue - orderValue) / orderValue * 100;
        return Math.Round(percentage, 2);
    }
}
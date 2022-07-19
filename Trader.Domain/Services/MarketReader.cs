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
        var isProfitable = CalculateIsProfitable(closedOrder, marketValue);
        var gainOrLossPercentage = CalculateGainOrLossPercentage(closedOrder, marketValue);

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

    private bool CalculateIsProfitable(ClosedOrder closedOrder, decimal marketValue)
    {
        //TODO factor in fees.

        if (closedOrder.OrderType == OrderType.Buy)
            return marketValue > closedOrder.Value;

        if (closedOrder.OrderType == OrderType.Sell)
            return marketValue < closedOrder.Value;

        throw new Exception("Unhandled OrderType");
    }

    private decimal CalculateGainOrLossPercentage(ClosedOrder closedOrder, decimal marketValue)
    {
        var orderValue = closedOrder.Value ?? 0;
        decimal percentage;

        if (closedOrder.OrderType == OrderType.Sell)
        {
            percentage = (orderValue - marketValue) / marketValue * 100;
        }
        else if (closedOrder.OrderType == OrderType.Buy)
        {
            percentage = (marketValue - orderValue) / orderValue * 100;
        }
        else 
        {
            throw new Exception("Unhandled OrderType");
        }
        return Math.Round(percentage, 2);
    }
}
using Trader.Domain.Enums;
using Trader.Domain.Models;
using Trader.Domain.InboundPorts;
using Trader.Domain.OutboundPorts;

namespace Trader.Domain.Services;

public class MarketReader : IMarketReader
{
    private readonly IMarketRepository _marketRepository;

    public MarketReader(IMarketRepository balancesRepository)
    {
        _marketRepository = balancesRepository;
    }

    public async Task<decimal> GetBitcoinLastPrice()
    {
        return await _marketRepository
            .GetLastPrice(CryptoCurrency.BTC, FiatCurrency.NZD);
    }

    public async Task<decimal> GetEtheriumLastPrice()
    {
        return await _marketRepository
            .GetLastPrice(CryptoCurrency.ETH, FiatCurrency.NZD);
    }

    public async Task<ClosedOrder> GetBitcoinLastClosedOrder()
    {
        return await _marketRepository
            .GetLastClosedOrder(CryptoCurrency.BTC, FiatCurrency.NZD);
    }

    public async Task<decimal> GetCurrentValueOfClosedOrder(ClosedOrder closedOrder)
    {
        return await GetCurrentValue(closedOrder.CryptoCurrency, closedOrder.Volume, closedOrder.FiatCurrency);
    }

    private async Task<decimal> GetCurrentValue(CryptoCurrency cryptoCurrency, decimal cryptoAmount, FiatCurrency fiatCurrency)
    {
        var currentPrice = await _marketRepository.GetLastPrice(cryptoCurrency, fiatCurrency);
        var currentValue = Math.Round(cryptoAmount * currentPrice, 2);

        return currentValue;
    }
}
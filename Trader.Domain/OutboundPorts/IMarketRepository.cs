using Trader.Domain.Enums;

namespace Trader.Domain.OutboundPorts;

public interface IMarketRepository
{
    Task<decimal> GetCurrentPrice(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency);

    Task PlaceBuyOrder(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency, decimal fiatAmount);
}
using Trader.Domain.Enums;

namespace Trader.Domain.OutboundPorts;

public interface IMarketRepository
{
    Task<decimal> GetLastPrice(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency);

    Task PlaceBuyOrder(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency, decimal fiatAmount);

    Task PlaceSellOrder(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency, decimal fiatAmount);
}
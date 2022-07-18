namespace Trader.Domain.OutboundPorts;

public interface IMarketRepository
{
    Task<decimal> GetLastMarketPrice(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency);

    Task PlaceBuyOrder(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency, decimal fiatAmount);

    Task PlaceSellOrder(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency, decimal fiatAmount);

    Task PlaceSellOrder(CryptoCurrency cryptoCurrency, decimal cryptoAmount, FiatCurrency fiatCurrency);

    Task<ClosedOrder> GetLastClosedOrder(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency);
}
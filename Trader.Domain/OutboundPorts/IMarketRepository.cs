namespace Trader.Domain.OutboundPorts;

public interface IMarketRepository
{
    Task<decimal> GetLastMarketPrice(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency);

    Task<ClosedOrder> GetLastClosedOrder(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency);

    Task PlaceBuyOrder(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency, decimal cryptoAmount);

    Task PlaceSellOrder(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency, decimal cryptoAmount);

    Task PlaceFiatBuyOrder(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency, decimal fiatAmount);

    Task PlaceFiatSellOrder(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency, decimal fiatAmount);
}
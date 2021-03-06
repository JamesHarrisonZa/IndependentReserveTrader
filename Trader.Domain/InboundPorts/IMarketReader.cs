namespace Trader.Domain.InboundPorts;

public interface IMarketReader
{
    Task<decimal> GetBitcoinLastPrice(FiatCurrency fiatCurrency);

    Task<decimal> GetEtheriumLastPrice(FiatCurrency fiatCurrency);

    Task<ClosedOrder> GetBitcoinLastClosedOrder();

    Task<MarketClosedOrder> GetMarketValueOfClosedOrder(ClosedOrder closedOrder);
}
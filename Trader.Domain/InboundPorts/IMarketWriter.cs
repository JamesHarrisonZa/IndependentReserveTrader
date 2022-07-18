namespace Trader.Domain.InboundPorts;

public interface IMarketWriter
{
    Task PlaceBitcoinFiatBuyOrder(decimal fiatAmount);

    Task PlaceBitcoinFiatSellOrder(decimal fiatAmount);

    Task PlaceBitcoinSellOrder(decimal btcAmount);
}
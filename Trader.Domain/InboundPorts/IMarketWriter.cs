namespace Trader.Domain.InboundPorts;

public interface IMarketWriter
{
    Task PlaceBitcoinBuyOrder(decimal fiatAmount);

    Task PlaceBitcoinSellOrder(decimal fiatAmount);
}
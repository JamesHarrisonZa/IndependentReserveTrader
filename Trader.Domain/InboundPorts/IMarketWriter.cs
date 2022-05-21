namespace Trader.Domain.InboundPorts;

public interface IMarketWriter
{
    Task PlaceBitcoinBuyOrder();

    // Task PlaceBitcoinSellOrder();
}
namespace Trader.Domain.InboundPorts;

public interface IMarketWriter
{

    Task PlaceBitcoinBuyOrder(decimal btcAmount);

    Task PlaceBitcoinSellOrder(decimal btcAmount);

    Task PlaceBitcoinFiatBuyOrder(decimal fiatAmount);

    Task PlaceBitcoinFiatSellOrder(decimal fiatAmount);
}
namespace Trader.Domain.InboundPorts;

public interface ITrader
{
    Task Trade();
}
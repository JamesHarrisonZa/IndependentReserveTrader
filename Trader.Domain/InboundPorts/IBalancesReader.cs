namespace Trader.Domain.InboundPorts;

public interface IBalancesReader
{
    double GetBitCoinBalance();
    double GetEtheriumCoinBalance();
}
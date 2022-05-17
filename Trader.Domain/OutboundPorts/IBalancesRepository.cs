namespace Trader.Domain.OutboundPorts;

public interface IBalancesRepository
{
    double GetBitCoinBalance();
    double GetEtheriumCoinBalance();
}
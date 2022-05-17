namespace Trader.Domain.InboundPorts;

public interface IBalancesReader
{
    Task<decimal> GetBitCoinBalance();

    Task<decimal> GetEtheriumCoinBalance();
}
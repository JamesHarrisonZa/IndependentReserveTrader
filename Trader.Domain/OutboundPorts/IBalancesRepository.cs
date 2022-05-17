namespace Trader.Domain.OutboundPorts;

public interface IBalancesRepository
{
    Task<decimal> GetBitCoinBalance();

    Task<decimal> GetEtheriumBalance();
}
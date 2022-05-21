namespace Trader.Domain.InboundPorts;

public interface IBalancesReader
{
    Task<decimal> GetBitCoinBalance();
    Task<decimal> GetBitCoinBalanceValue();

    Task<decimal> GetEtheriumBalance();
    Task<decimal> GetEtheriumBalanceValue();
}
namespace Trader.Domain.InboundPorts;

public interface IBalancesReader
{
    Task<decimal> GetBitcoinBalance();
    Task<decimal> GetBitcoinBalanceValue();

    Task<decimal> GetEtheriumBalance();
    Task<decimal> GetEtheriumBalanceValue();
}
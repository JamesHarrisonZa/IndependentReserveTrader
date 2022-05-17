namespace Trader.Domain.InboundPorts;

public interface IBalancesReader
{
    Task<decimal> GetBitCoinCurrentPrice();
    Task<decimal> GetBitCoinBalance();
    Task<decimal> GetBitCoinBalanceValue();

    Task<decimal> GetEtheriumCurrentPrice();
    Task<decimal> GetEtheriumBalance();
    Task<decimal> GetEtheriumBalanceValue();
}
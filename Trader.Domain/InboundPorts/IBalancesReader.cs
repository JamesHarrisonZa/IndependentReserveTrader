namespace Trader.Domain.InboundPorts;

public interface IBalancesReader
{
    Task<decimal> GetBitCoinBalance();

    Task<decimal> GetEtheriumBalance();

    Task<decimal> GetBitCoinCurrentPrice();
    
    Task<decimal> GetEtheriumCurrentPrice();
}
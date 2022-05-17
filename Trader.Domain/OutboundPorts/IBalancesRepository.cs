namespace Trader.Domain.OutboundPorts;

public interface IBalancesRepository
{
    Task<decimal> GetBitCoinBalance();

    Task<decimal> GetEtheriumBalance();

    Task<decimal> GetBitCoinCurrentPrice();
    
    Task<decimal> GetEtheriumCurrentPrice();
}
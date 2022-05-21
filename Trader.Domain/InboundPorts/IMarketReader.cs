namespace Trader.Domain.InboundPorts;

public interface IMarketReader
{
    Task<decimal> GetBitCoinCurrentPrice();

    Task<decimal> GetEtheriumCurrentPrice();
}
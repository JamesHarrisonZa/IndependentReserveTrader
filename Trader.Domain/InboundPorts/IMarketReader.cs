namespace Trader.Domain.InboundPorts;

public interface IMarketReader
{
    Task<decimal> GetBitCoinLastPrice();

    Task<decimal> GetEtheriumLastPrice();
}
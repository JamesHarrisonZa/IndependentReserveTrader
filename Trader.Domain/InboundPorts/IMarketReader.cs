namespace Trader.Domain.InboundPorts;

public interface IMarketReader
{
    Task<decimal> GetBitcoinLastPrice();

    Task<decimal> GetEtheriumLastPrice();
}
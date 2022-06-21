using Trader.Domain.Models;

namespace Trader.Domain.InboundPorts;

public interface IMarketReader
{
    Task<decimal> GetBitcoinLastPrice();

    Task<decimal> GetEtheriumLastPrice();
    
    Task<ClosedOrder> GetBitcoinLastClosedOrder();

    Task<decimal> GetCurrentValueOfClosedOrder(ClosedOrder closedOrder);
}
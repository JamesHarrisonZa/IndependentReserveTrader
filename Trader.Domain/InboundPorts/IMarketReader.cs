using Trader.Domain.Enums;
using Trader.Domain.Models;

namespace Trader.Domain.InboundPorts;

public interface IMarketReader
{
    Task<decimal> GetBitcoinLastPrice(FiatCurrency fiatCurrency);

    Task<decimal> GetEtheriumLastPrice(FiatCurrency fiatCurrency);
    
    Task<ClosedOrder> GetBitcoinLastClosedOrder();

    Task<decimal> GetCurrentValueOfClosedOrder(ClosedOrder closedOrder);
}
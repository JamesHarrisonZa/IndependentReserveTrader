namespace Trader.Adapter.IndependentReserve;

public static class OrderTypeConverter
{
    private static Dictionary<MarketOrderType, Domain.Models.OrderType> _orderTypes = 
        new Dictionary<MarketOrderType, Domain.Models.OrderType>()
        {
            { MarketOrderType.MarketBid, Domain.Models.OrderType.Buy},
            { MarketOrderType.LimitBid, Domain.Models.OrderType.Buy},
            { MarketOrderType.MarketOffer, Domain.Models.OrderType.Sell},
            { MarketOrderType.LimitOffer, Domain.Models.OrderType.Sell},
        };

    public static Domain.Models.OrderType GetOrderType(MarketOrderType marketOrderType)
    {
        return _orderTypes[marketOrderType];
    }
}
namespace Trader.Domain.Models;

public class MarketClosedOrder
{
    public CryptoCurrency CryptoCurrency { get; init; }
    public FiatCurrency FiatCurrency { get; init; }
    public decimal? ClosedOrderValue { get; init; }
    public decimal ClosedOrderVolume { get; init; }
    public decimal MarketValue { get; init; }
    public bool IsProfitable { get; init; }

    public MarketClosedOrder(ClosedOrder closedOrder)
    {
        CryptoCurrency = closedOrder.CryptoCurrency;
        FiatCurrency = closedOrder.FiatCurrency;
        ClosedOrderValue = closedOrder.Value;
        ClosedOrderVolume = closedOrder.Volume;
    }
}
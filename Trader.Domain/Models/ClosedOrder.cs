namespace Trader.Domain.Models;

public class ClosedOrder
{

    public DateTime CreatedUtc { get; set; }
    public OrderType OrderType { get; set; }
    public decimal Volume { get; set; }
    public decimal? Outstanding { get; set; }
    public decimal? Price { get; set; }
    public decimal? AvgPrice { get; set; }
    public decimal? Value { get; set; }
    // public OrderStatus Status { get; set; }
    public CryptoCurrency CryptoCurrency { get; set; }
    public FiatCurrency FiatCurrency { get; set; }
    public decimal FeePercent { get; set; }
    // public BankOrderVolume Original { get; set; }
}
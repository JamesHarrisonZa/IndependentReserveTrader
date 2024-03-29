namespace Trader.Adapter.IndependentReserve.Repositories;

public class MarketRepository : IMarketRepository
{
    private readonly IClient _client;

    public MarketRepository(IClient client)
    {
        _client = client;
    }

    public async Task<decimal> GetLastMarketPrice(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency)
    {
        var currencyCode = CodeConverter.GetCurrencyCode(cryptoCurrency);
        var fiatCurrencyCode = CodeConverter.GetCurrencyCode(fiatCurrency);

        var currencyCodeSummary = await _client
            .GetMarketSummaryAsync(currencyCode, fiatCurrencyCode); //ToDo: Share or add caching

        return currencyCodeSummary?.LastPrice ?? 0;
    }

    public async Task<ClosedOrder> GetLastClosedOrder(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency)
    {
        var currencyCode = CodeConverter.GetCurrencyCode(cryptoCurrency);
        var fiatCurrencyCode = CodeConverter.GetCurrencyCode(fiatCurrency);

        var closedOrders = await _client
            .GetClosedFilledOrdersAsync(currencyCode, fiatCurrencyCode, 1, 5); // Don't need to add pagination as most recent orders are returned first

        var lastClosedOrder = closedOrders.Data
            .OrderByDescending(o => o.CreatedTimestampUtc)
            .First();

        return new ClosedOrder
        {
            CreatedUtc = lastClosedOrder.CreatedTimestampUtc,
            OrderType = GetOrderType(lastClosedOrder),
            Volume = lastClosedOrder.Volume,
            Outstanding = lastClosedOrder.Outstanding,
            Price = lastClosedOrder.Price,
            AvgPrice = lastClosedOrder.AvgPrice,
            Value = lastClosedOrder.Value,
            CryptoCurrency = cryptoCurrency,
            FiatCurrency = fiatCurrency,
            FeePercent = lastClosedOrder.FeePercent,
        };
    }

    public async Task PlaceBuyOrder(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency, decimal cryptoAmount)
    {
        var currencyCode = CodeConverter.GetCurrencyCode(cryptoCurrency);
        var fiatCurrencyCode = CodeConverter.GetCurrencyCode(fiatCurrency);

        var response = await _client
            .PlaceMarketOrderAsync(currencyCode, fiatCurrencyCode, MarketOrderType.MarketBid, cryptoAmount);
    }

    public async Task PlaceSellOrder(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency, decimal cryptoAmount)
    {
        var currencyCode = CodeConverter.GetCurrencyCode(cryptoCurrency);
        var fiatCurrencyCode = CodeConverter.GetCurrencyCode(fiatCurrency);

        var response = await _client
            .PlaceMarketOrderAsync(currencyCode, fiatCurrencyCode, MarketOrderType.MarketOffer, cryptoAmount);
    }

    public async Task PlaceFiatBuyOrder(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency, decimal fiatAmount)
    {
        var currencyCode = CodeConverter.GetCurrencyCode(cryptoCurrency);
        var fiatCurrencyCode = CodeConverter.GetCurrencyCode(fiatCurrency);

        var cryptoAmount = await GetCryptoAmount(cryptoCurrency, fiatCurrency, fiatAmount);

        var response = await _client
            .PlaceMarketOrderAsync(currencyCode, fiatCurrencyCode, MarketOrderType.MarketBid, cryptoAmount);
    }

    public async Task PlaceFiatSellOrder(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency, decimal fiatAmount)
    {
        var currencyCode = CodeConverter.GetCurrencyCode(cryptoCurrency);
        var fiatCurrencyCode = CodeConverter.GetCurrencyCode(fiatCurrency);

        var cryptoAmount = await GetCryptoAmount(cryptoCurrency, fiatCurrency, fiatAmount);

        var response = await _client
            .PlaceMarketOrderAsync(currencyCode, fiatCurrencyCode, MarketOrderType.MarketOffer, cryptoAmount);
    }

    private async Task<decimal> GetCryptoAmount(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency, decimal fiatAmount)
    {
        var currentPrice = await GetLastMarketPrice(cryptoCurrency, fiatCurrency);

        var cryptoAmount = Math.Round(fiatAmount / currentPrice, 8);

        return cryptoAmount;
    }

    private Domain.Models.OrderType GetOrderType(BankHistoryOrder order)
    {
        return OrderTypeConverter.GetOrderType(order.OrderType);
    }
}

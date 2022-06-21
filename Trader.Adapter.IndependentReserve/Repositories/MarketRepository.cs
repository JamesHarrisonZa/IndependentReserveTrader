namespace Trader.Adapter.IndependentReserve.Repositories;

public class MarketRepository : IMarketRepository
{
    private readonly IClient _client;

    public MarketRepository(IClient client)
    {
        _client = client;
    }

    public async Task<decimal> GetLastPrice(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency)
    {
        var currencyCode = CodeConverter.GetCurrencyCode(cryptoCurrency);
        var fiatCurrencyCode = CodeConverter.GetCurrencyCode(fiatCurrency);

        var currencyCodeSummary = await _client
            .GetMarketSummaryAsync(currencyCode, fiatCurrencyCode); //ToDo: Share or add caching

        return currencyCodeSummary?.LastPrice ?? 0;
    }

    public async Task PlaceBuyOrder(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency, decimal fiatAmount)
    {
        var currencyCode = CodeConverter.GetCurrencyCode(cryptoCurrency);
        var fiatCurrencyCode = CodeConverter.GetCurrencyCode(fiatCurrency);

        var cryptoAmount = await GetCryptoAmount(cryptoCurrency, fiatCurrency, fiatAmount);

        Console.WriteLine($"Placing a buy order for {cryptoAmount}");

        var response = await _client
            .PlaceMarketOrderAsync(currencyCode, fiatCurrencyCode, OrderType.MarketBid, cryptoAmount);
    }

    public async Task PlaceSellOrder(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency, decimal fiatAmount)
    {
        var currencyCode = CodeConverter.GetCurrencyCode(cryptoCurrency);
        var fiatCurrencyCode = CodeConverter.GetCurrencyCode(fiatCurrency);

        var cryptoAmount = await GetCryptoAmount(cryptoCurrency, fiatCurrency, fiatAmount);

        Console.WriteLine($"Placing a sell order for {cryptoAmount}");

        var response = await _client
            .PlaceMarketOrderAsync(currencyCode, fiatCurrencyCode, OrderType.MarketOffer, cryptoAmount);
    }

    public async Task<ClosedOrder> GetLastClosedOrder(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency)
    {
        var currencyCode = CodeConverter.GetCurrencyCode(cryptoCurrency);
        var fiatCurrencyCode = CodeConverter.GetCurrencyCode(fiatCurrency);

        var closedOrders = await _client
            .GetClosedOrdersAsync(currencyCode, fiatCurrencyCode, 1, 25);

        var lastClosedOrder = closedOrders.Data
            .OrderByDescending(o => o.CreatedTimestampUtc)
            .First();

        //TODO mapping profile
        return new ClosedOrder
        {
            CreatedUtc = lastClosedOrder.CreatedTimestampUtc,
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

    private async Task<decimal> GetCryptoAmount(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency, decimal fiatAmount)
    {
        var currentPrice = await GetLastPrice(cryptoCurrency, fiatCurrency);

        var cryptoAmount = Math.Round(fiatAmount / currentPrice, 8);

        return cryptoAmount;
    }
}

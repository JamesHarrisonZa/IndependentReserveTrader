namespace Trader.Domain.Tests;

public class MarketReaderTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IMarketRepository> _marketRepository;
    private readonly IMarketReader _marketReader;

    public MarketReaderTests()
    {
        _fixture = new Fixture();
      _marketRepository = new Mock<IMarketRepository>();
      _marketReader = new MarketReader(_marketRepository.Object);
    }

    [Fact]
    public async void Given_FiatCurrency_When_GetBitcoinLastPrice_Then_Queries_Repository()
    {
        var fiatCurrency = FiatCurrency.NZD;
        var expectedLastPrice = 42m;
        SetupGetLastMarketPrice(CryptoCurrency.BTC, expectedLastPrice);

        var actualLastPrice = await _marketReader.GetBitcoinLastPrice(fiatCurrency);

        _marketRepository.Verify();
        Assert.Equal(expectedLastPrice, actualLastPrice);
    }

    [Fact]
    public async void Given_FiatCurrency_When_GetEtheriumLastPrice_Then_Queries_Repository()
    {
        var fiatCurrency = FiatCurrency.NZD;
        var expectedLastPrice = 42m;
        SetupGetLastMarketPrice(CryptoCurrency.ETH, expectedLastPrice);

        var actualLastPrice = await _marketReader.GetEtheriumLastPrice(fiatCurrency);

        _marketRepository.Verify();
        Assert.Equal(expectedLastPrice, actualLastPrice);
    }

    [Fact]
    public async void When_GetBitcoinLastClosedOrder_Then_Queries_Repository()
    {
        var expectedLastClosedOrder = _fixture
            .Create<ClosedOrder>();
        SetupGetLastClosedOrder(expectedLastClosedOrder);

        var actualLastClosedOrder = await _marketReader.GetBitcoinLastClosedOrder();

        _marketRepository.Verify();
        Assert.Equal(expectedLastClosedOrder, actualLastClosedOrder);
    }

    public static IEnumerable<object[]> MarketValueOfBuyClosedOrderData()
    {
        //Focus on orderVolumes
        yield return new object[] { 0.5, 10000, OrderType.Buy, 15000, 7500, false, -25 };
        yield return new object[] { 0.5, 10000, OrderType.Sell, 15000, 7500, true, 25 };
        yield return new object[] { 1, 10000, OrderType.Buy, 15000, 15000, true, 50 };
        yield return new object[] { 1, 10000, OrderType.Sell, 15000, 15000, false, -50 };
        yield return new object[] { 1.5, 10000, OrderType.Buy, 15000, 22500, true, 125 };
        yield return new object[] { 1.5, 10000, OrderType.Sell, 15000, 22500, false, -125 };

        // //Focus on lastPrices
        yield return new object[] { 1, 10000, OrderType.Buy, 8999.99m, 8999.99m, false, -10 };
        yield return new object[] { 1, 10000, OrderType.Sell, 8999.99m, 8999.99m, true, 10 };
        yield return new object[] { 1, 10000, OrderType.Buy, 10000, 10000, false, 0 };
        yield return new object[] { 1, 10000, OrderType.Buy, 11999.99, 11999.99m, true, 20 };
        yield return new object[] { 1, 10000, OrderType.Sell, 11999.99, 11999.99m, false, -20 };

        //Focus on GainOrLossPercentage
        yield return new object[] { 1, 100, OrderType.Buy, 90, 90, false, -10 };
        yield return new object[] { 1, 100, OrderType.Sell, 90, 90, true, 10 };
        yield return new object[] { 1, 100, OrderType.Buy, 100, 100, false, 0 };
        yield return new object[] { 1, 100, OrderType.Buy, 110, 110, true, 10 };
        yield return new object[] { 1, 100, OrderType.Sell, 110, 110, false, -10 };
    }

    [Theory]
    [MemberData(nameof(MarketValueOfBuyClosedOrderData))]
    public async void Given_ClosedOrder_When_GetMarketValueOfClosedOrder_Then_ReturnsMarketValue(
        decimal orderVolume, 
        decimal orderValue,
        OrderType orderType,
        decimal lastMarketPrice, 
        decimal expectedMarketValue,
        bool expectedIsProfitable,
        decimal expectedGainOrLossPercentage
    )
    {
        var closedOrder = _fixture
            .Build<ClosedOrder>()
            .With(co => co.OrderType, orderType)
            .With(co => co.Volume, orderVolume)
            .With(co => co.Value, orderValue)
            .Create();
        
        SetupGetLastMarketPrice(CryptoCurrency.BTC, lastMarketPrice);

        var marketClosedOrder = await _marketReader.GetMarketValueOfClosedOrder(closedOrder);

        _marketRepository.Verify();
        Assert.Equal(expectedMarketValue, marketClosedOrder.MarketValue);
        Assert.Equal(expectedIsProfitable, marketClosedOrder.IsProfitable);
        Assert.Equal(expectedGainOrLossPercentage, marketClosedOrder.GainOrLossPercentage);
    }

    private void SetupGetLastMarketPrice(CryptoCurrency cryptoCurrency, decimal lastPrice)
    {
        _marketRepository
            .Setup(br => br.GetLastMarketPrice(cryptoCurrency, FiatCurrency.NZD))
            .ReturnsAsync(lastPrice)
            .Verifiable();
    }

    private void SetupGetLastClosedOrder(ClosedOrder closedOrder)
    {
        _marketRepository
            .Setup(br => br.GetLastClosedOrder(CryptoCurrency.BTC, FiatCurrency.NZD))
            .ReturnsAsync(closedOrder)
            .Verifiable();
    }
}
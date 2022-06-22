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

    public static IEnumerable<object[]> GetCurrentValueOfClosedOrderData()
    {
        //Focus on orderVolumes
        yield return new object[] { 0.5, 10000, 15000, 7500, false, -25 };
        yield return new object[] { 1.0m, 10000.0m, 15000.0m, 15000.0m, true, 50 };
        yield return new object[] { 1.5m, 10000.0m, 15000.0m, 22500.00, true, 125 };

        // //Focus on lastPrices
        yield return new object[] { 1.0m, 10000.0m, 8999.99m, 8999.99m, false, -10 };
        yield return new object[] { 1.0m, 10000.0m, 10000.0m, 10000.0m, false, 0 };
        yield return new object[] { 1.0m, 10000.0m, 11999.99, 11999.99m, true, 20 };

        //Focus on GainOrLossPercentage
        yield return new object[] { 1, 100, 90, 90, false, -10 };
        yield return new object[] { 1, 100, 100, 100, false, 0 };
        yield return new object[] { 1, 100, 110, 110, true, 10 };
    }

    [Theory]
    [MemberData(nameof(GetCurrentValueOfClosedOrderData))]
    public async void Given_MarketReturnsPrice_When_GetMarketValueOfClosedOrder_Then_ReturnsMarketValue(
        decimal orderVolume, 
        decimal orderValue,
        decimal lastMarketPrice, 
        decimal expectedMarketValue,
        bool expectedIsProfitable,
        decimal expectedGainOrLossPercentage
    )
    {
        var closedOrder = _fixture
            .Build<ClosedOrder>()
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
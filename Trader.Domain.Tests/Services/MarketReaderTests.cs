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
        SetupGetLastPrice(CryptoCurrency.BTC, expectedLastPrice);

        var actualLastPrice = await _marketReader.GetBitcoinLastPrice(fiatCurrency);

        _marketRepository.Verify();
        Assert.Equal(expectedLastPrice, actualLastPrice);
    }

    [Fact]
    public async void Given_FiatCurrency_When_GetEtheriumLastPrice_Then_Queries_Repository()
    {
        var fiatCurrency = FiatCurrency.NZD;
        var expectedLastPrice = 42m;
        SetupGetLastPrice(CryptoCurrency.ETH, expectedLastPrice);

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
        yield return new object[] { 0.5m, 10000.0m, 15000.0m, 7500.00 };
        yield return new object[] { 1.0m, 10000.0m, 15000.0m, 15000.0m };
        yield return new object[] { 1.5m, 10000.0m, 15000.0m, 22500.00 };

        //Focus on lastPrices
        yield return new object[] { 1.0m, 10000.0m, 8999.99m, 8999.99m };
        yield return new object[] { 1.0m, 10000.0m, 10000.0m, 10000.0m };
        yield return new object[] { 1.0m, 10000.0m, 11999.99, 11999.99m };
    }

    [Theory]
    [MemberData(nameof(GetCurrentValueOfClosedOrderData))]
    public async void Given_MarketReturnsPrice_When_GetMarketValueOfClosedOrder_Then_ReturnsCurrentValue(
        decimal orderVolume, 
        decimal orderValue,
        decimal lastPrice, 
        decimal expectedCurrentValue
    )
    {
        var closedOrder = _fixture
            .Build<ClosedOrder>()
            .With(co => co.Volume, orderVolume)
            .With(co => co.Value, orderValue)
            .Create();
        
        SetupGetLastPrice(CryptoCurrency.BTC, lastPrice);

        var actualCurrentValue = await _marketReader.GetMarketValueOfClosedOrder(closedOrder);

        _marketRepository.Verify();
        Assert.Equal(expectedCurrentValue, actualCurrentValue);
    }

    private void SetupGetLastPrice(CryptoCurrency cryptoCurrency, decimal lastPrice)
    {
        _marketRepository
            .Setup(br => br.GetLastPrice(cryptoCurrency, FiatCurrency.NZD))
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
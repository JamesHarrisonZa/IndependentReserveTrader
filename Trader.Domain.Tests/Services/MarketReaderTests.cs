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
    public async void When_GetBitcoinLastPrice_Then_Queries_Repository()
    {
        var expectedLastPrice = 42m;
        SetupGetLastPrice(CryptoCurrency.BTC, expectedLastPrice);

        var actualLastPrice = await _marketReader.GetBitcoinLastPrice();

        _marketRepository.Verify();
        Assert.Equal(expectedLastPrice, actualLastPrice);
    }

    [Fact]
    public async void When_GetEtheriumLastPrice_Then_Queries_Repository()
    {
        var expectedLastPrice = 42m;
        SetupGetLastPrice(CryptoCurrency.ETH, expectedLastPrice);

        var actualLastPrice = await _marketReader.GetEtheriumLastPrice();

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
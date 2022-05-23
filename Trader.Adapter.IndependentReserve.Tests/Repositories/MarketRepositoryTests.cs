namespace Trader.Adapter.IndependentReserve.Tests;


public class MarketRepositoryTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IClient> _clientMock;
    private readonly IMarketRepository _marketRepository;

    public MarketRepositoryTests()
    {
        _fixture = new Fixture();

        _clientMock = new Mock<IClient>();

        _marketRepository = new MarketRepository(
            _clientMock.Object
        );
    }

    [Fact]
    public async void Given_CryptoCurrency_And_FiatCurrency_When_GetLastPrice_Then_Returns_LastPrice()
    {
        var cryptoCurrency = CryptoCurrency.BTC;
        var fiatCurrency = FiatCurrency.NZD;

        var expectedLastPrice = 42000m;
        var fakeMarketSummary = _fixture
            .Build<MarketSummary>()
            .With(ms => ms.LastPrice, expectedLastPrice)
            .Create();
        _clientMock
            .Setup(c => c.GetMarketSummaryAsync(CurrencyCode.Xbt, CurrencyCode.Nzd))
            .ReturnsAsync(fakeMarketSummary)
            .Verifiable();

        var actualLastPrice = await _marketRepository.GetLastPrice(cryptoCurrency, fiatCurrency);

        _clientMock.Verify();
        Assert.Equal(expectedLastPrice, actualLastPrice);
    }

    [Fact]
    public async void Given_CryptoCurrency_FiatCurrency_And_FiatAmount_When_PlaceBuyOrder_Then_PlacesOrder_With_MarketBid()
    {
        var cryptoCurrency = CryptoCurrency.BTC;
        var fiatCurrency = FiatCurrency.NZD;
        var fiatAmount = 420m;

        var fakeLastPrice = 42000m;
        var fakeMarketSummary = _fixture
            .Build<MarketSummary>()
            .With(ms => ms.LastPrice, fakeLastPrice)
            .Create();
        _clientMock
            .Setup(c => c
                .GetMarketSummaryAsync(CurrencyCode.Xbt, CurrencyCode.Nzd))
            .ReturnsAsync(fakeMarketSummary)
            .Verifiable();

        var expectedOrderType = OrderType.MarketBid;
        var expectedCryptoAmount = 0.01m;
        var fakeBankOrder = _fixture
            .Create<BankOrder>();
        _clientMock
            .Setup(c => c
                .PlaceMarketOrderAsync(
                  CurrencyCode.Xbt, 
                  CurrencyCode.Nzd, 
                  expectedOrderType, 
                  expectedCryptoAmount, 
                  null, 
                  null
            ))
            .ReturnsAsync(fakeBankOrder)
            .Verifiable();

        await _marketRepository
            .PlaceBuyOrder(cryptoCurrency, fiatCurrency, fiatAmount);

        _clientMock.VerifyAll();
    }
}
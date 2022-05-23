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

        Mock.VerifyAll();
        Assert.Equal(expectedLastPrice, actualLastPrice);
    }
}
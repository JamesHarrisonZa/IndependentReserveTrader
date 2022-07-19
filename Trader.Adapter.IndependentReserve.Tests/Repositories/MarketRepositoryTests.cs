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
    public async void Given_CryptoCurrency_And_FiatCurrency_When_GetLastMarketPrice_Then_Returns_LastPrice()
    {
        var cryptoCurrency = CryptoCurrency.BTC;
        var fiatCurrency = FiatCurrency.NZD;

        var expectedLastPrice = 42000m;
        SetupGetMarketSummary(expectedLastPrice);

        var actualLastPrice = await _marketRepository.GetLastMarketPrice(cryptoCurrency, fiatCurrency);

        _clientMock.Verify();
        Assert.Equal(expectedLastPrice, actualLastPrice);
    }

    [Fact]
    public async void Given_CryptoCurrency_FiatCurrency_When_GetLastClosedOrder_Then_Returns_OrderDetails()
    {
        var cryptoCurrency = CryptoCurrency.BTC;
        var fiatCurrency = FiatCurrency.NZD;

        var fakeClosedOrders = _fixture
            .Create<Page<BankHistoryOrder>>();
        _clientMock
            .Setup(c => c.GetClosedOrdersAsync(CurrencyCode.Xbt, CurrencyCode.Nzd, It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(fakeClosedOrders);

        var lastClosedOrder = await _marketRepository
            .GetLastClosedOrder(cryptoCurrency, fiatCurrency);

        _clientMock.VerifyAll();
    }


    [Fact]
    public async void Given_CryptoCurrency_FiatCurrency_And_CryptoAmount_When_PlaceBuyOrder_Then_PlacesOrder_With_MarketBid()
    {
        var cryptoCurrency = CryptoCurrency.BTC;
        var fiatCurrency = FiatCurrency.NZD;
        var cryptoAmount = 0.42m;

        var expectedOrderType = MarketOrderType.MarketBid;
        SetupPlaceMarketOrder(expectedOrderType, cryptoAmount);

        await _marketRepository
            .PlaceBuyOrder(cryptoCurrency, fiatCurrency, cryptoAmount);

        _clientMock.VerifyAll();
    }

    [Fact]
    public async void Given_CryptoCurrency_FiatCurrency_And_FiatAmount_When_PlaceFiatBuyOrder_Then_PlacesOrder_With_MarketBid()
    {
        var cryptoCurrency = CryptoCurrency.BTC;
        var fiatCurrency = FiatCurrency.NZD;
        var fiatAmount = 420m;

        var fakeLastPrice = 42000m;
        SetupGetMarketSummary(fakeLastPrice);

        var expectedOrderType = MarketOrderType.MarketBid;
        var expectedCryptoAmount = 0.01m;
        SetupPlaceMarketOrder(expectedOrderType, expectedCryptoAmount);

        await _marketRepository
            .PlaceFiatBuyOrder(cryptoCurrency, fiatCurrency, fiatAmount);

        _clientMock.VerifyAll();
    }

    [Fact]
    public async void Given_CryptoCurrency_FiatCurrency_And_CryptoAmount_When_PlaceSellOrder_Then_PlacesOrder_With_MarketBid()
    {
        var cryptoCurrency = CryptoCurrency.BTC;
        var fiatCurrency = FiatCurrency.NZD;
        var cryptoAmount = 0.42m;

        var expectedOrderType = MarketOrderType.MarketOffer;
        SetupPlaceMarketOrder(expectedOrderType, cryptoAmount);

        await _marketRepository
            .PlaceSellOrder(cryptoCurrency, fiatCurrency, cryptoAmount);

        _clientMock.VerifyAll();
    }

    [Fact]
    public async void Given_CryptoCurrency_FiatCurrency_And_FiatAmount_When_PlaceFiatSellOrder_Then_PlacesOrder_With_MarketOffer()
    {
        var cryptoCurrency = CryptoCurrency.BTC;
        var fiatCurrency = FiatCurrency.NZD;
        var fiatAmount = 420m;

        var fakeLastPrice = 42000m;
        SetupGetMarketSummary(fakeLastPrice);

        var expectedOrderType = MarketOrderType.MarketOffer;
        var expectedCryptoAmount = 0.01m;
        SetupPlaceMarketOrder(expectedOrderType, expectedCryptoAmount);

        await _marketRepository
            .PlaceFiatSellOrder(cryptoCurrency, fiatCurrency, fiatAmount);

        _clientMock.VerifyAll();
    }

    private MarketSummary GetFakeMarketSummary(decimal lastPrice)
    {
        return _fixture
            .Build<MarketSummary>()
            .With(ms => ms.LastPrice, lastPrice)
            .Create();
    }

    private void SetupGetMarketSummary(decimal lastPrice)
    {
        var fakeMarketSummary = GetFakeMarketSummary(lastPrice);

        _clientMock
            .Setup(c => c.GetMarketSummaryAsync(CurrencyCode.Xbt, CurrencyCode.Nzd))
            .ReturnsAsync(fakeMarketSummary)
            .Verifiable();
    }

    private void SetupPlaceMarketOrder(MarketOrderType expectedOrderType, decimal expectedCryptoAmount)
    {
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
    }
}
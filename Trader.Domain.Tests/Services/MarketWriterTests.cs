namespace Trader.Domain.Tests;

public class MarketWriterTests
{
    private readonly Mock<IMarketRepository> _marketRepository;
    private readonly MarketWriter _marketWriter;

    public MarketWriterTests()
    {
        _marketRepository = new Mock<IMarketRepository>();
        _marketWriter = new MarketWriter(_marketRepository.Object);
    }

    [Fact]
    public async void Given_FiatAmount_When_PlaceBitcoinFiatBuyOrder_Then_Calls_Repository()
    {
        var fiatAmount = 42m;
        SetupPlaceBuyOrder(CryptoCurrency.BTC, fiatAmount);

        await _marketWriter.PlaceBitcoinFiatBuyOrder(fiatAmount);

        _marketRepository.Verify();
    }

    [Fact]
    public async void Given_FiatAmount_When_PlaceBitcoinFiatSellOrder_Then_Calls_Repository()
    {
        var fiatAmount = 42m;
        SetupPlaceSellOrder(CryptoCurrency.BTC, fiatAmount);

        await _marketWriter.PlaceBitcoinFiatSellOrder(fiatAmount);

        _marketRepository.Verify();
    }

    private void SetupPlaceBuyOrder(CryptoCurrency cryptoCurrency, decimal fiatAmount)
    {
        _marketRepository
            .Setup(mr => mr.PlaceBuyOrder(cryptoCurrency, FiatCurrency.NZD, fiatAmount))
            .Verifiable();
    }

    private void SetupPlaceSellOrder(CryptoCurrency cryptoCurrency, decimal fiatAmount)
    {
        _marketRepository
            .Setup(mr => mr.PlaceSellOrder(cryptoCurrency, FiatCurrency.NZD, fiatAmount))
            .Verifiable();
    }
}
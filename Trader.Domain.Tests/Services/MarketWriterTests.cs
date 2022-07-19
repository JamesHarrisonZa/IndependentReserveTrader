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
    public async void Given_BtcAmount_When_PlaceBitcoinBuyOrder_Then_Calls_Repository()
    {
        var btcAmount = 42m;
        SetupPlaceBuyOrder(CryptoCurrency.BTC, btcAmount);

        await _marketWriter.PlaceBitcoinBuyOrder(btcAmount);

        _marketRepository.Verify();
    }

    [Fact]
    public async void Given_BtcAmount_When_PlaceBitcoinSellOrder_Then_Calls_Repository()
    {
        var btcAmount = 42m;
        SetupPlaceSellOrder(CryptoCurrency.BTC, btcAmount);

        await _marketWriter.PlaceBitcoinSellOrder(btcAmount);

        _marketRepository.Verify();
    }

    [Fact]
    public async void Given_FiatAmount_When_PlaceBitcoinFiatBuyOrder_Then_Calls_Repository()
    {
        var fiatAmount = 42m;
        SetupPlaceFiatBuyOrder(CryptoCurrency.BTC, fiatAmount);

        await _marketWriter.PlaceBitcoinFiatBuyOrder(fiatAmount);

        _marketRepository.Verify();
    }

    [Fact]
    public async void Given_FiatAmount_When_PlaceBitcoinFiatSellOrder_Then_Calls_Repository()
    {
        var fiatAmount = 42m;
        SetupPlaceFiatSellOrder(CryptoCurrency.BTC, fiatAmount);

        await _marketWriter.PlaceBitcoinFiatSellOrder(fiatAmount);

        _marketRepository.Verify();
    }

    private void SetupPlaceBuyOrder(CryptoCurrency cryptoCurrency, decimal btcAmount)
    {
        _marketRepository
            .Setup(mr => mr.PlaceBuyOrder(cryptoCurrency, FiatCurrency.NZD, btcAmount))
            .Verifiable();
    }

    private void SetupPlaceSellOrder(CryptoCurrency cryptoCurrency, decimal btcAmount)
    {
        _marketRepository
            .Setup(mr => mr.PlaceSellOrder(cryptoCurrency, FiatCurrency.NZD, btcAmount))
            .Verifiable();
    }

    private void SetupPlaceFiatBuyOrder(CryptoCurrency cryptoCurrency, decimal fiatAmount)
    {
        _marketRepository
            .Setup(mr => mr.PlaceFiatBuyOrder(cryptoCurrency, FiatCurrency.NZD, fiatAmount))
            .Verifiable();
    }

    private void SetupPlaceFiatSellOrder(CryptoCurrency cryptoCurrency, decimal fiatAmount)
    {
        _marketRepository
            .Setup(mr => mr.PlaceFiatSellOrder(cryptoCurrency, FiatCurrency.NZD, fiatAmount))
            .Verifiable();
    }
}
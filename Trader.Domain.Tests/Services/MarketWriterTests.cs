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
    public async void When_PlaceBitcoinBuyOrder_Then_Calls_Repository()
    {
        SetupPlaceBuyOrder(CryptoCurrency.BTC);

        await _marketWriter.PlaceBitcoinBuyOrder();

        _marketRepository.Verify();
    }

    private void SetupPlaceBuyOrder(CryptoCurrency cryptoCurrency)
    {
        _marketRepository
            .Setup(mr => mr.PlaceBuyOrder(cryptoCurrency, FiatCurrency.NZD, It.IsAny<decimal>()))
            .Verifiable();
    }
}
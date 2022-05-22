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
        var amount = 450m; //TODO. Plan coming soon™
        _marketRepository
            .Setup(mr => mr.PlaceBuyOrder(CryptoCurrency.BTC, FiatCurrency.NZD, amount))
            .Verifiable();

        await _marketWriter.PlaceBitcoinBuyOrder();

        _marketRepository.Verify();
    }
}
namespace Trader.Domain.Tests;

public class MarketReaderTests
{
    private readonly Mock<IMarketRepository> _marketRepository;
    private readonly IMarketReader _marketReader;

    public MarketReaderTests()
    {
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

    private void SetupGetLastPrice(CryptoCurrency cryptoCurrency, decimal lastPrice)
    {
        _marketRepository
            .Setup(br => br.GetLastPrice(cryptoCurrency, FiatCurrency.NZD))
            .ReturnsAsync(lastPrice)
            .Verifiable();
    }
}
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
    public async void When_GetBitCoinLastPrice_Then_Queries_Repository()
  {
    var expected = 42m;
    SetupGetLastPrice(CryptoCurrency.BTC, expected);

    var actual = await _marketReader.GetBitCoinLastPrice();

    _marketRepository.Verify();
    Assert.Equal(expected, actual);
  }

  [Fact]
    public async void When_GetEtheriumCurrentPrice_Then_Queries_Repository()
    {
        var expected = 42m;
        SetupGetLastPrice(CryptoCurrency.ETH, expected);

        var actual = await _marketReader.GetEtheriumCurrentPrice();

        _marketRepository.Verify();
        Assert.Equal(expected, actual);
    }

    private void SetupGetLastPrice(CryptoCurrency cryptoCurrency, decimal lastPrice)
    {
        _marketRepository
            .Setup(br => br.GetLastPrice(cryptoCurrency, FiatCurrency.NZD))
            .ReturnsAsync(lastPrice)
            .Verifiable();
    }
}
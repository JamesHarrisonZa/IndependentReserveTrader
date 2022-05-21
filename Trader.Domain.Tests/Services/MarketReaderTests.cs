using Moq;
using Xunit;
using Trader.Domain.Enums;
using Trader.Domain.Services;
using Trader.Domain.InboundPorts;
using Trader.Domain.OutboundPorts;

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
    public async void When_GetBitCoinCurrentPrice_Then_Queries_Repository()
    {
        var expected = 42m;
        _marketRepository
            .Setup(br => br.GetCurrentPrice(CryptoCurrency.BTC, FiatCurrency.NZD))
            .ReturnsAsync(expected)
            .Verifiable();

        var actual = await _marketReader.GetBitCoinCurrentPrice();

        _marketRepository.Verify();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async void When_GetEtheriumCurrentPrice_Then_Queries_Repository()
    {
        var expected = 42m;
        _marketRepository
            .Setup(br => br.GetCurrentPrice(CryptoCurrency.ETH, FiatCurrency.NZD))
            .ReturnsAsync(expected)
            .Verifiable();

        var actual = await _marketReader.GetEtheriumCurrentPrice();

        _marketRepository.Verify();
        Assert.Equal(expected, actual);
    }
}
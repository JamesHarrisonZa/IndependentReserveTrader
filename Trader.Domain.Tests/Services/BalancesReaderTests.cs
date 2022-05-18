using Moq;
using Xunit;
using Trader.Domain.Services;
using Trader.Domain.InboundPorts;
using Trader.Domain.OutboundPorts;

namespace Trader.Domain.Tests;

public class BalancesReaderTests
{
    private readonly Mock<IBalancesRepository> _balancesRepository;
    private readonly IBalancesReader _balancesReader;

    public BalancesReaderTests()
    {
      _balancesRepository = new Mock<IBalancesRepository>();
      _balancesReader = new BalancesReader(_balancesRepository.Object);
    }

    [Fact]
    public async void When_GetBitCoinBalance_Then_Queries_Repository()
    {
        var expected = 42m;
        _balancesRepository
            .Setup(br => br.GetCurrentPrice("BTC", "NZD"))
            .ReturnsAsync(expected);

        var actual = await _balancesReader.GetBitCoinCurrentPrice();

        Assert.Equal(expected, actual);
    }
}
using Moq;
using Xunit;
using Trader.Domain.Enums;
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
    public async void When_GetBitCoinCurrentPrice_Then_Queries_Repository()
    {
        var expected = 42m;
        _balancesRepository
            .Setup(br => br.GetCurrentPrice(CryptoCurrency.BTC, FiatCurrency.NZD))
            .ReturnsAsync(expected)
            .Verifiable();

        var actual = await _balancesReader.GetBitCoinCurrentPrice();

        _balancesRepository.Verify();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async void When_GetBitCoinBalance_Then_Queries_Repository()
    {
        var expected = 0.42m;
        _balancesRepository
            .Setup(br => br.GetBalance(CryptoCurrency.BTC))
            .ReturnsAsync(expected)
            .Verifiable();

        var actual = await _balancesReader.GetBitCoinBalance();

        _balancesRepository.Verify();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async void When_GetBitCoinBalanceValue_Then_Queries_Repository()
    {
        var expected = 4200m;
        _balancesRepository
            .Setup(br => br.GetBalanceValue(CryptoCurrency.BTC, FiatCurrency.NZD))
            .ReturnsAsync(expected)
            .Verifiable();

        var actual = await _balancesReader.GetBitCoinBalanceValue();

        _balancesRepository.Verify();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async void When_GetEtheriumCurrentPrice_Then_Queries_Repository()
    {
        var expected = 42m;
        _balancesRepository
            .Setup(br => br.GetCurrentPrice(CryptoCurrency.ETH, FiatCurrency.NZD))
            .ReturnsAsync(expected)
            .Verifiable();

        var actual = await _balancesReader.GetEtheriumCurrentPrice();

        _balancesRepository.Verify();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async void When_GetEtheriumBalance_Then_Queries_Repository()
    {
        var expected = 0.42m;
        _balancesRepository
            .Setup(br => br.GetBalance(CryptoCurrency.ETH))
            .ReturnsAsync(expected)
            .Verifiable();

        var actual = await _balancesReader.GetEtheriumBalance();

        _balancesRepository.Verify();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async void When_GetEtheriumBalanceValue_Then_Queries_Repository()
    {
        var expected = 4200m;
        _balancesRepository
            .Setup(br => br.GetBalanceValue(CryptoCurrency.ETH, FiatCurrency.NZD))
            .ReturnsAsync(expected)
            .Verifiable();

        var actual = await _balancesReader.GetEtheriumBalanceValue();

        _balancesRepository.Verify();
        Assert.Equal(expected, actual);
    }
}
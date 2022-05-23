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
    public async void When_GetBitcoinBalance_Then_Queries_Repository()
    {
        var expectedBalance = 0.42m;
        SetupGetBalance(CryptoCurrency.BTC, expectedBalance);

        var actualBalance = await _balancesReader.GetBitcoinBalance();

        _balancesRepository.Verify();
        Assert.Equal(expectedBalance, actualBalance);
    }

    [Fact]
    public async void When_GetBitcoinBalanceValue_Then_Queries_Repository()
    {
        var expectedBalanceValue = 4200m;
        SetupGetBalanceValue(CryptoCurrency.BTC, expectedBalanceValue);

        var actualBalanceValue = await _balancesReader.GetBitcoinBalanceValue();

        _balancesRepository.Verify();
        Assert.Equal(expectedBalanceValue, actualBalanceValue);
    }

    [Fact]
    public async void When_GetEtheriumBalance_Then_Queries_Repository()
    {
        var expectedBalance = 0.42m;
        SetupGetBalance(CryptoCurrency.ETH, expectedBalance);

        var actualBalance = await _balancesReader.GetEtheriumBalance();

        _balancesRepository.Verify();
        Assert.Equal(expectedBalance, actualBalance);
    }

    [Fact]
    public async void When_GetEtheriumBalanceValue_Then_Queries_Repository()
    {
        var expectedBalanceValue = 4200m;
        SetupGetBalanceValue(CryptoCurrency.ETH, expectedBalanceValue);

        var actualBalanceValue = await _balancesReader.GetEtheriumBalanceValue();

        _balancesRepository.Verify();
        Assert.Equal(expectedBalanceValue, actualBalanceValue);
    }

    private void SetupGetBalance(CryptoCurrency cryptoCurrency, decimal balance)
    {
        _balancesRepository
            .Setup(br => br.GetBalance(cryptoCurrency))
            .ReturnsAsync(balance)
            .Verifiable();
    }

    private void SetupGetBalanceValue(CryptoCurrency cryptoCurrency, decimal balanceValue)
    {
        _balancesRepository
            .Setup(br => br.GetBalanceValue(cryptoCurrency, FiatCurrency.NZD))
            .ReturnsAsync(balanceValue)
            .Verifiable();
    }
}
namespace Trader.Adapter.IndependentReserve.Tests;

public class BalancesRepositoryTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IMarketRepository> _marketRepositoryMock;
    private readonly Mock<IClient> _clientMock;
    private readonly Mock<MyMemoryCache> _memoryCache;
    private readonly IBalancesRepository _balancesRepository;

    public BalancesRepositoryTests()
    {
        _fixture = new Fixture();
        _marketRepositoryMock = new Mock<IMarketRepository>();
        _clientMock = new Mock<IClient>();
        _memoryCache = new Mock<MyMemoryCache>();

        _balancesRepository = new BalancesRepository(
            _marketRepositoryMock.Object,
            _clientMock.Object,
            _memoryCache.Object
        );
    }

    [Fact]
    public async void Given_CryptoCurrency_When_GetBalance_Then_Returns_Balance()
    {
        var cryptoCurrency = CryptoCurrency.BTC;

        var expectedBalance = 0.42m;
        SetupGetAccounts(expectedBalance);

        var actualBalance = await _balancesRepository
            .GetBalance(cryptoCurrency);

        _clientMock.Verify();
        Assert.Equal(expectedBalance, actualBalance);
    }

    [Fact]
    public async void Given_CryptoCurrency_And_FiatCurrency_When_GetBalanceValue_Then_Returns_GetBalanceValue()
    {
        var cryptoCurrency = CryptoCurrency.BTC;
        var fiatCurrency = FiatCurrency.NZD;

        var balance = 0.42m;
        SetupGetAccounts(balance);

        var lastPrice = 42000m;
        SetupGetLastMarketPrice(cryptoCurrency, fiatCurrency, lastPrice);

        var expectedBalanceValue = 17640m;

        var actualBalanceValue = await _balancesRepository
            .GetBalanceValue(cryptoCurrency, fiatCurrency);

        _clientMock.Verify();
        _marketRepositoryMock.Verify();
        Assert.Equal(expectedBalanceValue, actualBalanceValue);
    }

    private void SetupGetLastMarketPrice(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency, decimal lastPrice)
    {
        _marketRepositoryMock
            .Setup(mr => mr.GetLastMarketPrice(cryptoCurrency, fiatCurrency))
            .ReturnsAsync(lastPrice)
            .Verifiable();
    }

    private Account GetFakeBtcAccount(decimal balance)
    {
        return _fixture
            .Build<Account>()
            .With(a => a.CurrencyCode, CurrencyCode.Xbt)
            .With(a => a.AvailableBalance, balance)
            .Create();
    }

    private void SetupGetAccounts(decimal balance)
    {
        var fakeAccount = GetFakeBtcAccount(balance);

        _clientMock
            .Setup(c => c.GetAccountsAsync())
            .ReturnsAsync(new List<Account> { fakeAccount })
            .Verifiable();
    }
}
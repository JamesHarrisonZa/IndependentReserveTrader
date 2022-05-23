namespace Trader.Adapter.IndependentReserve.Tests;

public class BalancesRepositoryTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IMarketRepository> _marketRepositoryMock;
    private readonly Mock<IClient> _clientMock;
    private readonly IBalancesRepository _balancesRepository;

    public BalancesRepositoryTests()
    {
        _fixture = new Fixture();

        _marketRepositoryMock = new Mock<IMarketRepository>();

        _clientMock = new Mock<IClient>();

        _balancesRepository = new BalancesRepository(
            _marketRepositoryMock.Object, 
            _clientMock.Object
        );
    }

    [Fact]
    public async void Given_CryptoCurrency_When_GetBalance_Then_Returns_Balance()
    {
        var cryptoCurrency = CryptoCurrency.BTC;

        var expectedBalance = 0.42m;
        var fakeAccount = GetFakeBtcAccount(expectedBalance);
        _clientMock
            .Setup(c => c.GetAccountsAsync())
            .ReturnsAsync(new List<Account> { fakeAccount })
            .Verifiable();

        var actualBalance = await _balancesRepository
            .GetBalance(cryptoCurrency);

        Mock.VerifyAll();
        Assert.Equal(expectedBalance, actualBalance);
    }

  [Fact]
    public async void Given_CryptoCurrency_And_FiatCurrency_When_GetBalanceValue_Then_Returns_GetBalanceValue()
    {
        var cryptoCurrency = CryptoCurrency.BTC;
        var fiatCurrency = FiatCurrency.NZD;

        var fakeAccount = GetFakeBtcAccount(0.42m);
        _clientMock
            .Setup(c => c.GetAccountsAsync())
            .ReturnsAsync(new List<Account> { fakeAccount })
            .Verifiable();

        _marketRepositoryMock
            .Setup(mr => mr.GetLastPrice(cryptoCurrency, fiatCurrency))
            .ReturnsAsync(42000m)
            .Verifiable();
        
        var expectedBalanceValue = 17640m;

        var actualBalanceValue = await _balancesRepository
            .GetBalanceValue(cryptoCurrency, fiatCurrency);

        Mock.VerifyAll();
        Assert.Equal(expectedBalanceValue, actualBalanceValue);
    }

    private Account GetFakeBtcAccount(decimal balance)
    {
        return _fixture
            .Build<Account>()
            .With(a => a.CurrencyCode, CurrencyCode.Xbt)
            .With(a => a.AvailableBalance, balance)
            .Create();
    }
}
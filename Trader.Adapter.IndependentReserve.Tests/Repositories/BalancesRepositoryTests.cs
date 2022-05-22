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
        
        var expected = 0.42m;

        var fakeAccount = _fixture
            .Build<Account>()
            .With(a => a.CurrencyCode, CurrencyCode.Xbt)
            .With(a => a.AvailableBalance, expected)
            .Create();

        _clientMock
            .Setup(c => c.GetAccountsAsync())
            .ReturnsAsync(new List<Account>{ fakeAccount });

        var actual = await _balancesRepository
            .GetBalance(cryptoCurrency);

        Assert.Equal(expected, actual);
    }
}
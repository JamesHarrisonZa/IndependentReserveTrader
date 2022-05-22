namespace Trader.Adapter.IndependentReserve.Tests;

public class BalancesRepositoryTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IMarketRepository> _marketRepositoryMock;
    private readonly Mock<Client> _clientMock;
    private readonly IBalancesRepository _balancesRepository;

    public BalancesRepositoryTests()
    {
        _fixture = new Fixture();

        _marketRepositoryMock = new Mock<IMarketRepository>();

        //Cant mock this... Thanks Moq. Plan coming soon™
        //_clientMock = new Mock<Client>();

        // _balancesRepository = new BalancesRepository(
        //     _marketRepositoryMock.Object, 
        //     _clientMock.Object
        // );
    }

    [Fact]
    public void Given_CryptoCurrency_When_GetBalance_Then_Returns_Balance()
    {
        var cryptoCurrency = CryptoCurrency.BTC;

        var fakeResponse = _fixture
            .Create<IEnumerable<Account>>();

        // _clientMock
        //     .Setup(c => c.GetAccountsAsync())
        //     .ReturnsAsync(fakeResponse);

        // var expected = _balancesRepository
        //     .GetBalance(cryptoCurrency);

        // Assert.NotNull(expected);
    }
}
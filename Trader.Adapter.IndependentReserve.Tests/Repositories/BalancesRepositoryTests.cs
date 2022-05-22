namespace Trader.Adapter.IndependentReserve.Tests;

public class BalancesRepositoryTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IMarketRepository> _marketRepositoryMock;
    private readonly IBalancesRepository _balancesRepository;

    public BalancesRepositoryTests()
    {
        _fixture = new Fixture();
        var independentReserveConfigFake = _fixture
            .Build<IndependentReserveConfig>()
            .With(c => c.BaseUrl, "https://example.com")
            .Create();

        _marketRepositoryMock = new Mock<IMarketRepository>();

        _balancesRepository = new BalancesRepository(
            _marketRepositoryMock.Object, 
            independentReserveConfigFake
        );
    }

    [Fact]
    public void Given_CryptoCurrency_When_GetBalance_Then_Returns_Balance()
    {
        var cryptoCurrency = CryptoCurrency.BTC;
    }
}
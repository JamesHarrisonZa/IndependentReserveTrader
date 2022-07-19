namespace Trader.Domain.Tests;

public class TraderTests
{
    private readonly Mock<IMarketReader> _marketReader;
    private readonly Mock<IMarketWriter> _marketWriter;
    private readonly ITradingConfig _tradingConfig;
    private readonly ITrader _trader;

    public TraderTests()
    {
        _marketReader = new Mock<IMarketReader>();
        _marketWriter = new Mock<IMarketWriter>();
        _tradingConfig = new TradingConfig();

        _trader = new Trader.Domain.Services.Trader(_marketReader.Object, _marketWriter.Object, _tradingConfig);
    }

    [Fact]
    public async void Given_LastClosedOrderWasBuy_And_MarketIsAboveTrigger_When_Trading_Then_Sells()
    {
        
    }

    [Fact]
    public async void Given_LastClosedOrderWasSell_And_MarketIsBelowTrigger_When_Trading_Then_Buys()
    {
        
    }
}
namespace Trader.Domain.Tests;

public class TraderTests
{
    private readonly Mock<IMarketReader> _marketReader;
    private readonly Mock<IMarketWriter> _marketWriter;
    private readonly ITradingConfig _tradingConfig;
    private readonly Fixture _fixture;
    private readonly ITrader _trader;

    public TraderTests()
    {
        _marketReader = new Mock<IMarketReader>();
        _marketWriter = new Mock<IMarketWriter>();
        _tradingConfig = new TradingConfig();
        _fixture = new Fixture();

        _trader = new Trader.Domain.Services.Trader(_marketReader.Object, _marketWriter.Object, _tradingConfig);
    }

    [Theory]
    [InlineData(100, 111, 11, 0.42)]
    [Trait("Scenario", "LastClosedOrderWasBuy")]
    [Trait("Scenario", "IsProfitable")]
    public async void Given_LastClosedOrderWasBuy_IsProfitable_And_LastClosedOrderGain_Is_HigherThanTrigger_When_Trading_Then_Sells(
      decimal orderValue, 
      decimal marketOrderValue,
      decimal gainPercentage,
      decimal orderVolume
    )
    {
        var orderType = OrderType.Buy;
        var isProfitable = true;

        var marketClosedOrder = BuildMarketClosedOrder(orderType, isProfitable, orderValue, marketOrderValue, gainPercentage, orderVolume);
        SetupMarketReader(marketClosedOrder);

        await _trader.Trade();

        VerifySellOrder(orderVolume);
    }

    [Theory]
    [InlineData(100, 109, 9)]
    [Trait("Scenario", "LastClosedOrderWasBuy")]
    [Trait("Scenario", "IsProfitable")]
    public async void Given_LastClosedOrderWasBuy_IsProfitable_And_LastClosedOrderGain_IsNot_HigherThanTrigger_When_Trading_Then_Waits
    (
      decimal orderValue, 
      decimal marketOrderValue,
      decimal gainPercentage
    )
    {
        var orderType = OrderType.Buy;
        var isProfitable = true;

        var marketClosedOrder = BuildMarketClosedOrder(orderType, isProfitable, orderValue, marketOrderValue, gainPercentage);
        SetupMarketReader(marketClosedOrder);

        await _trader.Trade();

        VerifyDoesNotBuyOrSell();
    }

    [Theory]
    [InlineData(100, 89, 11, 0.42)]
    [Trait("Scenario", "LastClosedOrderWasSell")]
    [Trait("Scenario", "IsProfitable")]
    public async void Given_LastClosedOrderWasSell_IsProfitable_And_LastClosedOrderGain_Is_HigherThanTrigger_When_Trading_Then_Buys(
      decimal orderValue, 
      decimal marketOrderValue,
      decimal gainPercentage,
      decimal orderVolume
    )
    {
        var orderType = OrderType.Sell;
        var isProfitable = true;

        var marketClosedOrder = BuildMarketClosedOrder(orderType, isProfitable, orderValue, marketOrderValue, gainPercentage, orderVolume);
        SetupMarketReader(marketClosedOrder);

        await _trader.Trade();

        VerifyBuyOrder(orderVolume);
    }

    [Theory]
    [InlineData(100, 91, 9)]
    [Trait("Scenario", "LastClosedOrderWasSell")]
    [Trait("Scenario", "IsProfitable")]
    public async void Given_LastClosedOrderWasSell_IsProfitable_And_LastClosedOrderGain_IsNot_HigherThanTrigger_When_Trading_Then_Waits
    (
      decimal orderValue, 
      decimal marketOrderValue,
      decimal gainPercentage
    )
    {
        var orderType = OrderType.Sell;
        var isProfitable = true;

        var marketClosedOrder = BuildMarketClosedOrder(orderType, isProfitable, orderValue, marketOrderValue, gainPercentage);
        SetupMarketReader(marketClosedOrder);

        await _trader.Trade();

        VerifyDoesNotBuyOrSell();
    }

    private MarketClosedOrder BuildMarketClosedOrder(
      OrderType orderType, 
      bool isProfitable, 
      decimal orderValue, 
      decimal marketOrderValue, 
      decimal gainPercentage, 
      decimal orderVolume = 0
    )
    {
        return _fixture
            .Build<MarketClosedOrder>()
            .With(mco => mco.OrderType, orderType)
            .With(mco => mco.IsProfitable, isProfitable)
            .With(mco => mco.ClosedOrderValue, orderValue)
            .With(mco => mco.MarketValue, marketOrderValue)
            .With(mco => mco.GainOrLossPercentage, gainPercentage)
            .With(mco => mco.ClosedOrderVolume, orderVolume)
            .Create();
    }

    private void SetupMarketReader(MarketClosedOrder marketClosedOrder)
    {
        var closedOrder = _fixture
            .Build<ClosedOrder>()
            .Create();

        _marketReader
            .Setup(mr => mr.GetBitcoinLastClosedOrder())
            .ReturnsAsync(closedOrder)
            .Verifiable();

        _marketReader
            .Setup(mr => mr.GetMarketValueOfClosedOrder(closedOrder))
            .ReturnsAsync(marketClosedOrder)
            .Verifiable();
    }

    private void VerifySellOrder(decimal orderVolume)
    {
          _marketWriter
              .Verify(mw => mw.PlaceBitcoinSellOrder(orderVolume), Times.Once());
    }

    private void VerifyBuyOrder(decimal orderVolume)
    {
          _marketWriter
              .Verify(mw => mw.PlaceBitcoinBuyOrder(orderVolume), Times.Once());
    }

    private void VerifyDoesNotBuyOrSell()
    {
          _marketWriter
              .Verify(mw => mw.PlaceBitcoinBuyOrder(It.IsAny<decimal>()), Times.Never());

          _marketWriter
              .Verify(mw => mw.PlaceBitcoinSellOrder(It.IsAny<decimal>()), Times.Never());
    }
}
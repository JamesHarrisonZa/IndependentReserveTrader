namespace Trader.Domain.Services;

public class Trader: ITrader
{
    private readonly IMarketReader _marketReader;
    private readonly IMarketWriter _marketWriter;
    private readonly IConfig _config;

    public Trader(IMarketReader marketReader, IMarketWriter marketWriter, IConfig config)
    {
        _marketReader = marketReader;
        _marketWriter = marketWriter;
        _config = config;
    }

    public async Task Trade()
    {
        var lastClosedOrder = await _marketReader.GetBitcoinLastClosedOrder();
        var marketClosedOrder = await _marketReader.GetMarketValueOfClosedOrder(lastClosedOrder);

        if (ShouldSell(marketClosedOrder))
        {
          // await _marketWriter.PlaceBitcoinSellOrder(marketClosedOrder.ClosedOrderVolume);
          AnsiConsole.MarkupLine($"[{Color.Blue}]Selling amount {marketClosedOrder.ClosedOrderVolume} for around {marketClosedOrder.MarketValue}. Increase of {marketClosedOrder.GainOrLossPercentage}% [/] 🚀🚀🚀");
        }
        else
        {
          AnsiConsole.MarkupLine($"[{Color.Orange1}] Waiting for opportunity to sell {_config.GainTriggerPercentage - marketClosedOrder.GainOrLossPercentage}% away [/] 🎯");
        }
    }

    private bool ShouldSell(MarketClosedOrder marketClosedOrder)
    {
        if (marketClosedOrder.OrderType == OrderType.Sell)
            return false;

        if (!marketClosedOrder.IsProfitable)
            return false;

        if (marketClosedOrder.GainOrLossPercentage < _config.GainTriggerPercentage)
            return false;

        return true;
    }
}
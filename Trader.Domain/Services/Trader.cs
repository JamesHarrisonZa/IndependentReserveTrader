namespace Trader.Domain.Services;

public class Trader: ITrader
{
    private readonly IMarketReader _marketReader;
    private readonly IMarketWriter _marketWriter;
    private readonly ITradingConfig _config;

    public Trader(IMarketReader marketReader, IMarketWriter marketWriter, ITradingConfig config)
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
            AnsiConsole.MarkupLine($"[{Color.Blue}]Selling amount {marketClosedOrder.ClosedOrderVolume} for around {marketClosedOrder.MarketValue}. Increase of {marketClosedOrder.GainOrLossPercentage}% [/] 💲💲💲📈🚀");
        }
        else if (ShouldBuy(marketClosedOrder))
        {
            //await _marketWriter.PlaceBitcoinBuyOrder(marketClosedOrder.ClosedOrderVolume);
            AnsiConsole.MarkupLine($"[{Color.Blue}]Buying amount {marketClosedOrder.ClosedOrderVolume} for around {marketClosedOrder.MarketValue}. Market has dropped {marketClosedOrder.GainOrLossPercentage}% [/] 🤲🤲🤲📉🙏");
        }
        else 
        {
            var percentageToTrigger = GetPercentageToTrigger(marketClosedOrder);
            AnsiConsole.MarkupLine($"[{Color.Orange1}] Waiting for opportunity {_config.GainTriggerPercentage - marketClosedOrder.GainOrLossPercentage}% away [/] 🎯");
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

    private bool ShouldBuy(MarketClosedOrder marketClosedOrder)
    {
        if (marketClosedOrder.OrderType == OrderType.Buy)
            return false;

        if (!marketClosedOrder.IsProfitable)
            return false;

        if (marketClosedOrder.GainOrLossPercentage < _config.LossTriggerPercentage)
            return false;

        return true;
    }

    private decimal GetPercentageToTrigger(MarketClosedOrder marketClosedOrder)
    {
        if (marketClosedOrder.OrderType == OrderType.Sell)
            return _config.LossTriggerPercentage - marketClosedOrder.GainOrLossPercentage;

        if (marketClosedOrder.OrderType == OrderType.Buy)
            return _config.GainTriggerPercentage - marketClosedOrder.GainOrLossPercentage;

        throw new Exception("Unhandled OrderType");
    }
}
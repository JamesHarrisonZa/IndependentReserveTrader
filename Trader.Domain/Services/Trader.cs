namespace Trader.Domain.Services;

public class Trader: ITrader
{
    private readonly IMarketReader _marketReader;
    private readonly IMarketWriter _marketWriter;
    private const decimal _targetPercentage = 10;

    public Trader(IMarketReader marketReader, IMarketWriter marketWriter)
    {
        _marketReader = marketReader;
        _marketWriter = marketWriter;
    }

    public async Task Trade()
    {
        var lastClosedOrder = await _marketReader.GetBitcoinLastClosedOrder();
        var marketClosedOrder = await _marketReader.GetMarketValueOfClosedOrder(lastClosedOrder);

        if (marketClosedOrder.IsProfitable && marketClosedOrder.GainOrLossPercentage > _targetPercentage)
        {
            // await _marketWriter.PlaceBitcoinSellOrder(marketClosedOrder.ClosedOrderVolume);
            AnsiConsole.MarkupLine($"[{Color.Green}]Selling amount {marketClosedOrder.ClosedOrderVolume} for around {marketClosedOrder.MarketValue}. Increase of {marketClosedOrder.GainOrLossPercentage}% [/] 🚀🚀🚀");
        }
        else 
        {
            AnsiConsole.MarkupLine($"[{Color.Orange1}] Waiting for target... {_targetPercentage - marketClosedOrder.GainOrLossPercentage}% away [/] 🎯");
        }

    }
}
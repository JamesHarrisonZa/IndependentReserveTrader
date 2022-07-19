namespace Trader.Domain.Config;

public class TradingConfig : ITradingConfig
{
    public decimal GainTriggerPercentage => 10;

    public decimal LossTriggerPercentage => 10;
}
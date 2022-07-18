namespace Trader.Domain.Config;

public class Config : IConfig
{
    public decimal GainTriggerPercentage => 10;

    public decimal LossTriggerPercentage => 10;
}
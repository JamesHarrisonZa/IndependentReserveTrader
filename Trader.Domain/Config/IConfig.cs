namespace Trader.Domain.Config;

public interface ITradingConfig
{
    decimal GainTriggerPercentage { get; }
    decimal LossTriggerPercentage { get; }
}
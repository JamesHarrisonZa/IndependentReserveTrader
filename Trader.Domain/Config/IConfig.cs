namespace Trader.Domain.Config;

public interface IConfig
{
    decimal GainTriggerPercentage { get; }
    decimal LossTriggerPercentage { get; }
}
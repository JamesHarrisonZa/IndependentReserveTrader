﻿namespace Trader.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static void AddDomainPorts(this IServiceCollection services)
    {
        services.AddSingleton<IBalancesReader, BalancesReader>();
        services.AddSingleton<IMarketReader, MarketReader>();
        services.AddSingleton<IMarketWriter, MarketWriter>();
        services.AddSingleton<ITrader, Trader.Domain.Services.Trader>();
        services.AddSingleton<ITradingConfig, TradingConfig>();
    }
}
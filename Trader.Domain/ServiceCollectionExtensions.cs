namespace Trader.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddDomainPorts(this IServiceCollection services)
    {
        services.AddSingleton<IBalancesReader, BalancesReader>();
        services.AddSingleton<IMarketReader, MarketReader>();
        services.AddSingleton<IMarketWriter, MarketWriter>();
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trader.Domain.OutboundPorts;
using Trader.Adapter.IndependentReserve.Config;
using Trader.Adapter.IndependentReserve.Repositories;

namespace Trader.Adapter.IndependentReserve;

public static class ServiceCollectionExtensions
{
    public static void AddIndependentReserveAdapter(this IServiceCollection services, IConfiguration configuration)
    {
        var independentReserveConfig = configuration.GetSection("IndependentReserve").Get<IndependentReserveConfig>();
        services.AddSingleton<IndependentReserveConfig>(independentReserveConfig);

        services.AddSingleton<IBalancesRepository, BalancesRepository>();
        services.AddSingleton<IMarketRepository, MarketRepository>();
    }
}
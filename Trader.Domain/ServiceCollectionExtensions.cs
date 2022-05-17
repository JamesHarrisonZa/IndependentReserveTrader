using Microsoft.Extensions.DependencyInjection;
using Trader.Domain.Services;
using Trader.Domain.InboundPorts;

namespace Trader.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddDomainPorts(this IServiceCollection services)
    {
        services.AddSingleton<IBalancesReader, BalancesReader>();
    }
}
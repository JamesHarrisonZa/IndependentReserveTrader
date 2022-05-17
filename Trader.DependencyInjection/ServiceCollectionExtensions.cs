using Microsoft.Extensions.DependencyInjection;
using Trader.Adapter.IndependentReserve;

namespace Trader.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddDomainPorts();
        services.AddIndependentReserveAdapter();
    }
}
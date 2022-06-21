namespace Trader.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddTraderServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDomainPorts();
        services.AddIndependentReserveAdapter(configuration);
    }
}
namespace Trader.Adapter.IndependentReserve;

public static class ServiceCollectionExtensions
{
    public static void AddIndependentReserveAdapter(this IServiceCollection services, IConfiguration configuration)
    {
        var independentReserveConfig = configuration.GetSection("IndependentReserve").Get<IndependentReserveConfig>();
        var apiConfig = new ApiConfig(independentReserveConfig.BaseUrl, independentReserveConfig.ApiKey, independentReserveConfig.ApiSecret);
        var client = Client.Create(apiConfig);
        services.AddSingleton<IClient>(client);

        services.AddSingleton<IBalancesRepository, BalancesRepository>();
        services.AddSingleton<IMarketRepository, MarketRepository>();
    }
}
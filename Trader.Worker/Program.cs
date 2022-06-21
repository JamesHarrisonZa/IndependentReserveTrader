var env = System.Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

var configuration = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
    .Build();

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IConfiguration>(configuration);
        services.AddHostedService<Worker>();
        services.AddTraderServices(configuration);
    })
    .Build();

await host.RunAsync();

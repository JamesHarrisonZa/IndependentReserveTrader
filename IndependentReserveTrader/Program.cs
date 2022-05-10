using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using IndependentReserveTrader;

// Build a config object, using env vars and JSON providers.
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

// Get values from the config given their key and their target type.
var independentReserveConfig = config
    .GetRequiredSection("IndependentReserve")
    .Get<IndependentReserveConfig>();

Console.WriteLine(independentReserveConfig.ApiKey);
Console.WriteLine(independentReserveConfig.ApiSecret);

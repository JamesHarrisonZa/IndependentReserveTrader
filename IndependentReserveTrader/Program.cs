using Microsoft.Extensions.Configuration;
using IndependentReserve.DotNetClientApi;
using IndependentReserve.DotNetClientApi.Data;
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

// Console.WriteLine(independentReserveConfig.ApiKey);
// Console.WriteLine(independentReserveConfig.ApiSecret);

var apiConfig = new ApiConfig(independentReserveConfig.BaseUrl, independentReserveConfig.ApiKey, independentReserveConfig.ApiSecret);
var client = Client.Create(apiConfig);

var btcSummary = client.GetMarketSummary(CurrencyCode.Xbt, CurrencyCode.Usd);
Console.WriteLine("BTC Last price:" + btcSummary.LastPrice);

var accounts = await client.GetAccountsAsync();

var btcAccount = accounts.FirstOrDefault(a => a.CurrencyCode == CurrencyCode.Xbt);
Console.WriteLine("My BTC balance:" + btcAccount?.TotalBalance);

var ethAccount = accounts.FirstOrDefault(a => a.CurrencyCode == CurrencyCode.Eth);
Console.WriteLine("My ETH balance:" + ethAccount?.TotalBalance);
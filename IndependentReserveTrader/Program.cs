using Microsoft.Extensions.Configuration;
using IndependentReserve.DotNetClientApi;
using IndependentReserve.DotNetClientApi.Data;
using IndependentReserveTrader;

var env = System.Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

var config = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
    .Build();

var independentReserveConfig = config
    .GetRequiredSection("IndependentReserve")
    .Get<IndependentReserveConfig>();

var apiConfig = new ApiConfig(independentReserveConfig.BaseUrl, independentReserveConfig.ApiKey, independentReserveConfig.ApiSecret);
var client = Client.Create(apiConfig);

var btcSummary = client.GetMarketSummary(CurrencyCode.Xbt, CurrencyCode.Usd);
Console.WriteLine("BTC Last price:" + btcSummary.LastPrice);

var accounts = await client.GetAccountsAsync();

var btcAccount = accounts.FirstOrDefault(a => a.CurrencyCode == CurrencyCode.Xbt);
Console.WriteLine("My BTC balance:" + btcAccount?.TotalBalance);

var ethAccount = accounts.FirstOrDefault(a => a.CurrencyCode == CurrencyCode.Eth);
Console.WriteLine("My ETH balance:" + ethAccount?.TotalBalance);
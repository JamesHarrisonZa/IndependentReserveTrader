global using IndependentReserve.DotNetClientApi;
global using IndependentReserve.DotNetClientApi.Data;
global using MarketOrderType = IndependentReserve.DotNetClientApi.Data.OrderType;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.DependencyInjection;

global using Trader.Domain.Enums;
global using Trader.Domain.Models;
global using Trader.Domain.OutboundPorts;
global using Trader.Adapter.IndependentReserve.Cache;
global using Trader.Adapter.IndependentReserve.Config;
global using Trader.Adapter.IndependentReserve.Repositories;
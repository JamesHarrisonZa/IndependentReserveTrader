# Independent Reserve Trader

## ⚡ Getting started

```bash
dotnet run --project Trader.Worker/Trader.Worker.csproj
```

```bash
# Code Coverage
# Using https://www.nuget.org/packages/dotnet-reportgenerator-globaltool

dotnet test Trader.sln --collect:"XPlat Code Coverage"

dotnet reportgenerator -reports:"./**/coverage.cobertura.xml" -targetdir:"./CoverageReport" -reporttypes:"html"
```

## 🎩 Patterns

- __Hexagonal architecture__ with __Ports & Adapters__ pattern.
  - [Netflix post](https://netflixtechblog.com/ready-for-changes-with-hexagonal-architecture-b315ec967749)

## 🤝 Third party docs

- Independent Reserve API
  - [API Docs](https://www.independentreserve.com/nz/products/api)
  - [DotNet API Client on github](https://github.com/independentreserve/dotNetApiClient)

- Spectre.Console
  - [Nuget package on github](https://github.com/spectreconsole/spectre.console)
  - [Docs](https://spectreconsole.net/)

## 🔧 Work in progress™

- Domain logic for algorithm to buy/sell 🤡

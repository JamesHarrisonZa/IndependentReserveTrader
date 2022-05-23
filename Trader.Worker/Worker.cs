using Spectre.Console;
using Trader.Domain.InboundPorts;

namespace Trader.Worker;

public class Worker : BackgroundService
{
    private readonly IBalancesReader _balancesReader;
    private readonly IMarketReader _marketReader;
    private readonly IMarketWriter _marketWriter;
    private readonly ILogger<Worker> _logger;

    public Worker(IBalancesReader balancesReader, IMarketReader marketReader, IMarketWriter marketWriter, ILogger<Worker> logger)
    {
        _balancesReader = balancesReader;
        _marketReader = marketReader;
        _marketWriter = marketWriter;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var balancesTable = await GetBalancesTable();
        AnsiConsole.Write(balancesTable);

        await Update(stoppingToken);
    }

    private async Task<Table> GetBalancesTable()
    {
        var btcBalance = await _balancesReader.GetBitCoinBalance();
        var ethBalance = await _balancesReader.GetEtheriumBalance();

        var btcCurrentPrice = await _marketReader.GetBitCoinLastPrice();
        var ethCurrentPrice = await _marketReader.GetEtheriumCurrentPrice();

        var btcValue = await _balancesReader.GetBitCoinBalanceValue();
        var ethValue = await _balancesReader.GetEtheriumBalanceValue();

        var table = new Table();

        table.AddColumn("Coin");
        table.AddColumn(new TableColumn("Current Price (NZD)").Centered());
        table.AddColumn(new TableColumn("Balance").Centered());
        table.AddColumn(new TableColumn("Value (NZD)").Centered());

        table.AddRow("[cyan1]BTC[/]", $"[cyan1]{btcCurrentPrice}[/]", $"[cyan1]{btcBalance}[/]", $"[cyan1]{btcValue}[/]");
        table.AddRow("[chartreuse2]ETH[/]", $"[chartreuse2]{ethCurrentPrice}[/]", $"[chartreuse2]{ethBalance}[/]", $"[chartreuse2]{ethValue}[/]");

        return table;
    }

    private async Task Update(CancellationToken stoppingToken)
    {
        await AnsiConsole.Status()
              .Spinner(Spinner.Known.Aesthetic)
              .SpinnerStyle(Style.Parse("green"))
              .StartAsync("Doing stuff... 🚀", async ctx =>
              {
                  while (!stoppingToken.IsCancellationRequested)
                  {
                      // AnsiConsole.MarkupLine("[bold]Doing something clever 🧠[/]");

                      var fiveSeconds = 5000;
                      await Task.Delay(fiveSeconds, stoppingToken);
                  }
              });
    }
}

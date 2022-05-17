using Spectre.Console;
using Trader.Domain.OutboundPorts;

namespace Trader.Worker;

public class Worker : BackgroundService
{
    private readonly IBalancesRepository _balancesRepository;
    private readonly ILogger<Worker> _logger;

    public Worker(IBalancesRepository balancesRepository, ILogger<Worker> logger)
    {
        _balancesRepository = balancesRepository;
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
        var btcBalance = await _balancesRepository.GetBitCoinBalance();
        var ethBalance = await _balancesRepository.GetEtheriumBalance();

        var btcCurrentPrice = await _balancesRepository.GetBitCoinCurrentPrice();
        var ethCurrentPrice = await _balancesRepository.GetEtheriumCurrentPrice();

        var btcValue = Math.Round(btcBalance * btcCurrentPrice, 2);
        var ethValue = Math.Round(ethBalance * ethCurrentPrice, 2);

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
                    AnsiConsole.MarkupLine("[bold]Doing something clever 🧠[/]");

                    var fiveSeconds = 5000;
                    await Task.Delay(fiveSeconds, stoppingToken);
                }
              });
    }
}

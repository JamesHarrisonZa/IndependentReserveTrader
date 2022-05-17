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

    private async Task<Table> GetBalancesTable()
    {
        var bitCoinBalance = await _balancesRepository.GetBitCoinBalance();
        var etheriumCoinBalance = await _balancesRepository.GetEtheriumCoinBalance();

        var table = new Table();

        table.AddColumn("Coin");
        table.AddColumn(new TableColumn("Balance").Centered());

        table.AddRow("[cyan1]BTC[/]", $"[cyan1]{bitCoinBalance}[/]");
        table.AddRow("[chartreuse2]ETH[/]", $"[chartreuse2]{etheriumCoinBalance}[/]");

        return table;
    }
}

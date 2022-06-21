using Spectre.Console;
using Trader.Domain.Models;
using Trader.Domain.InboundPorts;
using Trader.Domain.Enums;

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
        var lastClosedOrder = await _marketReader.GetBitcoinLastClosedOrder();

        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Aesthetic)
            .SpinnerStyle(Style.Parse("green"))
            .StartAsync("Comparing assets against market 🚀🙏", async ctx =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Update(lastClosedOrder);

                    var fiveSeconds = 5000;
                    await Task.Delay(fiveSeconds, stoppingToken);
                }
            });
    }

    private async Task Update(ClosedOrder lastClosedOrder)
    {
        AnsiConsole.Clear();

        await WriteBalancesTable();
        await WriteLastOrderBarChart(lastClosedOrder);

        WriteLastUpdated();
    }

    private async Task WriteBalancesTable()
    {
        var btcBalance = await _balancesReader.GetBitcoinBalance();
        var ethBalance = await _balancesReader.GetEtheriumBalance();

        var btcCurrentPrice = await _marketReader.GetBitcoinLastPrice(FiatCurrency.NZD);
        var ethCurrentPrice = await _marketReader.GetEtheriumLastPrice(FiatCurrency.NZD);

        var btcValue = await _balancesReader.GetBitcoinBalanceValue();
        var ethValue = await _balancesReader.GetEtheriumBalanceValue();

        var table = new Table();

        table.AddColumn("Coin");
        table.AddColumn(new TableColumn("Current Price (NZD)").Centered());
        table.AddColumn(new TableColumn("Balance").Centered());
        table.AddColumn(new TableColumn("Value (NZD)").Centered());

        table.AddRow("[cyan1]BTC[/]", $"[cyan1]{btcCurrentPrice}[/]", $"[cyan1]{btcBalance}[/]", $"[cyan1]{btcValue}[/]");
        table.AddRow("[chartreuse2]ETH[/]", $"[chartreuse2]{ethCurrentPrice}[/]", $"[chartreuse2]{ethBalance}[/]", $"[chartreuse2]{ethValue}[/]");

        AnsiConsole.Write(table);
    }

    private async Task WriteLastOrderBarChart(ClosedOrder lastClosedOrder)
    {
        var currentValue = await _marketReader.GetCurrentValueOfClosedOrder(lastClosedOrder);

        var lastOrderValue = Convert.ToDouble(lastClosedOrder.Value);
        var orderMarketValue = Convert.ToDouble(currentValue);
        var profitOrLossColour = orderMarketValue > lastOrderValue
            ? Color.Green
            : Color.Red;

        var barChart = new BarChart()
            .Width(60)
            .Label($"[green bold underline]Last Bitcoin Order {lastClosedOrder.FiatCurrency}[/]")
            .CenterLabel();

        barChart.AddItem("[cyan1] Order Value [/]", lastOrderValue, Color.Yellow1);
        barChart.AddItem("[cyan1] Market Value [/]", orderMarketValue, profitOrLossColour);

        AnsiConsole.Write(barChart);
    }

    private static void WriteLastUpdated()
    {
        var lastUpdated = DateTime.Now.ToString("hh:mm:ss tt");
        AnsiConsole.MarkupLine($"Last updated⌚: [bold]{lastUpdated}[/]");
    }

    private void WriteLastBitcoinOrder(ClosedOrder lastClosedOrder)
    {
        AnsiConsole.MarkupLine(@$"
            💲[bold]Last Bitcoin Order[/]💲
              [cyan1]Created:[/] [yellow1]{lastClosedOrder.CreatedUtc}[/],
              [cyan1]Volume:[/] [yellow1]{lastClosedOrder.Volume}[/],
              [cyan1]Value:[/] [yellow1]{lastClosedOrder.Value} {lastClosedOrder.FiatCurrency}[/],
              [cyan1]AvgPrice:[/] [yellow1]{lastClosedOrder.AvgPrice}[/],
              [cyan1]FeePercent:[/] [yellow1]{lastClosedOrder.FeePercent}[/],
              [cyan1]Outstanding:[/] [yellow1]{lastClosedOrder.Outstanding}[/],
              ");
    }
}

namespace Trader.Worker;

public class Worker : BackgroundService
{
    private readonly IBalancesReader _balancesReader;
    private readonly IMarketReader _marketReader;
    private readonly IMarketWriter _marketWriter;
    private readonly ILogger<Worker> _logger;
    private const int _updateDelayTime =  1 * 60 * 1000; //1 minute

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

                    await Task.Delay(_updateDelayTime, stoppingToken);
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

        var btcMarketPriceUsd = await _marketReader.GetBitcoinLastPrice(FiatCurrency.USD);
        var btcMarketPriceNzd = await _marketReader.GetBitcoinLastPrice(FiatCurrency.NZD);
        var ethMarketPriceUsd = await _marketReader.GetEtheriumLastPrice(FiatCurrency.USD);
        var ethMarketPriceNzd = await _marketReader.GetEtheriumLastPrice(FiatCurrency.NZD);

        var btcValue = await _balancesReader.GetBitcoinBalanceValue();
        var ethValue = await _balancesReader.GetEtheriumBalanceValue();

        var table = new Table();

        table.AddColumn("Coin");
        table.AddColumn(new TableColumn("Market Price (USD)").Centered());
        table.AddColumn(new TableColumn("Market Price (NZD)").Centered());
        table.AddColumn(new TableColumn("Balance").Centered());
        table.AddColumn(new TableColumn("Value (NZD)").Centered());

        table.AddRow("[cyan1]BTC[/]", $"[cyan1]{btcMarketPriceUsd}[/]", $"[cyan1]{btcMarketPriceNzd}[/]", $"[cyan1]{btcBalance}[/]", $"[cyan1]{btcValue}[/]");
        table.AddRow("[chartreuse2]ETH[/]", $"[chartreuse2]{ethMarketPriceUsd}[/]", $"[chartreuse2]{ethMarketPriceNzd}[/]", $"[chartreuse2]{ethBalance}[/]", $"[chartreuse2]{ethValue}[/]");

        AnsiConsole.Write(table);
    }

    private async Task WriteLastOrderBarChart(ClosedOrder lastClosedOrder)
  {
    var marketClosedOrder = await _marketReader.GetMarketValueOfClosedOrder(lastClosedOrder);

    var lastOrderValue = Convert.ToDouble(lastClosedOrder.Value);
    var orderMarketValue = Convert.ToDouble(marketClosedOrder.MarketValue);
    var profitOrLossColour = GetProfitOrLossColour(marketClosedOrder);

    var barChart = new BarChart()
        .Width(60)
        .Label($"[green bold underline]Last Bitcoin Order {lastClosedOrder.FiatCurrency}[/]")
        .CenterLabel();

    barChart.AddItem("[cyan1] Order Value [/]", lastOrderValue, Color.Yellow1);
    barChart.AddItem("[cyan1] Market Value [/]", orderMarketValue, profitOrLossColour);
    AnsiConsole.Write(barChart);

    WriteGainOrLossPercentage(marketClosedOrder);
  }

  private static Color GetProfitOrLossColour(MarketClosedOrder marketClosedOrder)
  {
      return marketClosedOrder.IsProfitable
          ? Color.Green
          : Color.Red;
  }

  private static void WriteGainOrLossPercentage(MarketClosedOrder marketClosedOrder)
  {
      var profitOrLossColour = GetProfitOrLossColour(marketClosedOrder);

      var emojis = marketClosedOrder.IsProfitable
          ? "📈💲😎👍"
          : "📉💸😭👎";

      AnsiConsole.MarkupLine("");
      AnsiConsole.MarkupLine($"[{profitOrLossColour}]Gain or Loss Percentage: {marketClosedOrder.GainOrLossPercentage}% [/] {emojis}");
  }

  private static void WriteLastUpdated()
    {
        var lastUpdated = DateTime.Now.ToString("hh:mm:ss tt");
        var nextUpdate = DateTime.Now.AddMilliseconds(_updateDelayTime).ToString("hh:mm:ss tt");
        AnsiConsole.MarkupLine("");
        AnsiConsole.MarkupLine($"Last updated⌚: [bold]{lastUpdated}[/], next update: [bold]{nextUpdate}[/]");
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

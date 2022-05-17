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
        while (!stoppingToken.IsCancellationRequested)
        {
            var bitCoinBalance = await _balancesRepository.GetBitCoinBalance();
            _logger.LogInformation($"BitCoin balance: ${bitCoinBalance}");

            var etheriumCoinBalance = await _balancesRepository.GetEtheriumCoinBalance();
            _logger.LogInformation($"Etherium balance: ${etheriumCoinBalance}");

            // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            var fiveSeconds = 5000;
            await Task.Delay(fiveSeconds, stoppingToken);
        }
    }
}

using Trader.Domain.InboundPorts;
using Trader.Domain.OutboundPorts;

namespace Trader.Domain.Services;

public class BalancesReader : IBalancesReader
{
    private readonly IBalancesRepository _balancesRepository;

    public BalancesReader(IBalancesRepository balancesRepository)
    {
        _balancesRepository = balancesRepository;
    }

    public async Task<decimal> GetBitCoinBalance()
    {
      return await _balancesRepository.GetBitCoinBalance();
    }

    public async Task<decimal> GetEtheriumCoinBalance()
    {
      return await _balancesRepository.GetEtheriumCoinBalance();
    }
}
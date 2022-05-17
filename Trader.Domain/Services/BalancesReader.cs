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

    public async Task<decimal> GetEtheriumBalance()
    {
      return await _balancesRepository.GetEtheriumBalance();
    }

    public async Task<decimal> GetBitCoinCurrentPrice()
    {
      return await _balancesRepository.GetBitCoinCurrentPrice();
    }
    
    public async Task<decimal> GetEtheriumCurrentPrice()
    {
      return await _balancesRepository.GetEtheriumCurrentPrice();
    }
}
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

    public async Task<decimal> GetBitCoinCurrentPrice()
    {
      return await _balancesRepository.GetCurrentPrice("BTC", "NZD");
    }

    public async Task<decimal> GetBitCoinBalance()
    {
      return await _balancesRepository.GetBalance("BTC");
    }

    public async Task<decimal> GetBitCoinBalanceValue()
    {
      return await _balancesRepository.GetBalanceValue("BTC", "NZD");
    }
    
    public async Task<decimal> GetEtheriumCurrentPrice()
    {
      return await _balancesRepository.GetCurrentPrice("ETH", "NZD");
    }

    public async Task<decimal> GetEtheriumBalance()
    {
      return await _balancesRepository.GetBalance("ETH");
    }

    public async Task<decimal> GetEtheriumBalanceValue()
    {
      return await _balancesRepository.GetBalanceValue("ETH", "NZD");
    }
}
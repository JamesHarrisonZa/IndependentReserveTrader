using Trader.Domain.Enums;
using Trader.Domain.InboundPorts;
using Trader.Domain.OutboundPorts;

namespace Trader.Domain.Services;

public class MarketReader : IMarketReader
{
    private readonly IMarketRepository _marketRepository;

    public MarketReader(IMarketRepository balancesRepository)
    {
        _marketRepository = balancesRepository;
    }

    public async Task<decimal> GetBitCoinCurrentPrice()
    {
      return await _marketRepository
          .GetCurrentPrice(CryptoCurrency.BTC, FiatCurrency.NZD);
    }
    
    public async Task<decimal> GetEtheriumCurrentPrice()
    {
      return await _marketRepository
          .GetCurrentPrice(CryptoCurrency.ETH, FiatCurrency.NZD);
    }
}
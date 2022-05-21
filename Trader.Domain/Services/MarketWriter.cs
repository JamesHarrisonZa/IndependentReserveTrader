using Trader.Domain.Enums;
using Trader.Domain.InboundPorts;
using Trader.Domain.OutboundPorts;

namespace Trader.Domain.Services;

public class MarketWriter : IMarketWriter
{
    private readonly IMarketRepository _marketRepository;

    public MarketWriter(IMarketRepository balancesRepository)
    {
        _marketRepository = balancesRepository;
    }

    public async Task PlaceBitcoinBuyOrder()
    {
      var amount = 950m; //TODO. Plan coming soon™

      await _marketRepository
          .PlaceBuyOrder(CryptoCurrency.BTC, FiatCurrency.NZD, amount);
    }
}
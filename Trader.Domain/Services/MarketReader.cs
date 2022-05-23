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

    public async Task<decimal> GetBitCoinLastPrice()
    {
        return await _marketRepository
            .GetLastPrice(CryptoCurrency.BTC, FiatCurrency.NZD);
    }

    public async Task<decimal> GetEtheriumLastPrice()
    {
        return await _marketRepository
            .GetLastPrice(CryptoCurrency.ETH, FiatCurrency.NZD);
    }
}
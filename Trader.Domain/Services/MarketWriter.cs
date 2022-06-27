namespace Trader.Domain.Services;

public class MarketWriter : IMarketWriter
{
    private readonly IMarketRepository _marketRepository;

    public MarketWriter(IMarketRepository balancesRepository)
    {
        _marketRepository = balancesRepository;
    }

    public async Task PlaceBitcoinBuyOrder(decimal fiatAmount)
    {
        await _marketRepository
            .PlaceBuyOrder(CryptoCurrency.BTC, FiatCurrency.NZD, fiatAmount);
    }

    public async Task PlaceBitcoinSellOrder(decimal fiatAmount)
    {
        await _marketRepository
            .PlaceSellOrder(CryptoCurrency.BTC, FiatCurrency.NZD, fiatAmount);
    }
}
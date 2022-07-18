namespace Trader.Domain.Services;

public class MarketWriter : IMarketWriter
{
    private readonly IMarketRepository _marketRepository;

    public MarketWriter(IMarketRepository balancesRepository)
    {
        _marketRepository = balancesRepository;
    }

    public async Task PlaceBitcoinFiatBuyOrder(decimal fiatAmount)
    {
        await _marketRepository
            .PlaceBuyOrder(CryptoCurrency.BTC, FiatCurrency.NZD, fiatAmount);
    }

    public async Task PlaceBitcoinFiatSellOrder(decimal fiatAmount)
    {
        await _marketRepository
            .PlaceSellOrder(CryptoCurrency.BTC, FiatCurrency.NZD, fiatAmount);
    }

    public async Task PlaceBitcoinSellOrder(decimal btcAmount)
    {
        await _marketRepository
            .PlaceSellOrder(CryptoCurrency.BTC, btcAmount, FiatCurrency.NZD);
    }
}
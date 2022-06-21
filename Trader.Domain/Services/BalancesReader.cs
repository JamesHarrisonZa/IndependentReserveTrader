namespace Trader.Domain.Services;

public class BalancesReader : IBalancesReader
{
    private readonly IBalancesRepository _balancesRepository;

    public BalancesReader(IBalancesRepository balancesRepository)
    {
        _balancesRepository = balancesRepository;
    }

    public async Task<decimal> GetBitcoinBalance()
    {
        return await _balancesRepository
            .GetBalance(CryptoCurrency.BTC);
    }

    public async Task<decimal> GetBitcoinBalanceValue()
    {
        return await _balancesRepository
            .GetBalanceValue(CryptoCurrency.BTC, FiatCurrency.NZD);
    }

    public async Task<decimal> GetEtheriumBalance()
    {
        return await _balancesRepository
            .GetBalance(CryptoCurrency.ETH);
    }

    public async Task<decimal> GetEtheriumBalanceValue()
    {
        return await _balancesRepository
            .GetBalanceValue(CryptoCurrency.ETH, FiatCurrency.NZD);
    }
}
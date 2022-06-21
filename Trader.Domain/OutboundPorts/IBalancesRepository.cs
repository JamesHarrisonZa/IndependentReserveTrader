namespace Trader.Domain.OutboundPorts;

public interface IBalancesRepository
{
    Task<decimal> GetBalance(CryptoCurrency cryptoCurrency);

    Task<decimal> GetBalanceValue(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency);
}
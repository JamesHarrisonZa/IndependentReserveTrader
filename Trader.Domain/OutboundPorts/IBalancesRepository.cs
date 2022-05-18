using Trader.Domain.Enums;

namespace Trader.Domain.OutboundPorts;

public interface IBalancesRepository
{
    Task<decimal> GetBalance(CryptoCurrency cryptoCurrency);

    Task<decimal> GetCurrentPrice(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency);
    
    Task<decimal> GetBalanceValue(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency);
}
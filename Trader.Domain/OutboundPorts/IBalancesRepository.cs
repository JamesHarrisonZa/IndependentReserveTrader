namespace Trader.Domain.OutboundPorts;

public interface IBalancesRepository
{
    Task<decimal> GetBalance(string code);

    Task<decimal> GetCurrentPrice(string code, string fiatCurrency);
    
    Task<decimal> GetBalanceValue(string code, string fiatCurrency);
}
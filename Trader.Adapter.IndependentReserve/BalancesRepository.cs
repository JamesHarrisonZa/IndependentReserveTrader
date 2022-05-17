namespace Trader.Adapter.IndependentReserve;

public interface BalancesRepository
{
    double GetBitCoinBalance();
    double GetEtheriumCoinBalance();
}
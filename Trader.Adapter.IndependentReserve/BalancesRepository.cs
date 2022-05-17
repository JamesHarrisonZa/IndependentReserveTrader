using Trader.Domain.OutboundPorts;

namespace Trader.Adapter.IndependentReserve;

public class BalancesRepository : IBalancesRepository
{
    public double GetBitCoinBalance()
    {
        return 42;
    }

    public double GetEtheriumCoinBalance()
    {
        return 42;
    }
}
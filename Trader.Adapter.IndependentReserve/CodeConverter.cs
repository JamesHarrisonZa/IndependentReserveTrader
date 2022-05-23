using IndependentReserve.DotNetClientApi.Data;
using Trader.Domain.Enums;

namespace Trader.Adapter.IndependentReserve;

public static class CodeConverter
{

    public static CurrencyCode GetCurrencyCode(CryptoCurrency code)
    {
        switch (code)
        {
            case CryptoCurrency.BTC:
                return CurrencyCode.Xbt;
            case CryptoCurrency.ETH:
                return CurrencyCode.Eth;

            default:
                throw new ArgumentException($"Invalid code: {code}");
        }
    }

    public static CurrencyCode GetCurrencyCode(FiatCurrency code)
    {
        switch (code)
        {
            case FiatCurrency.NZD:
                return CurrencyCode.Nzd;
            case FiatCurrency.USD:
                return CurrencyCode.Usd;

            default:
                throw new ArgumentException($"Invalid code: {code}");
        }
    }
}
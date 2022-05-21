using IndependentReserve.DotNetClientApi.Data;
using Trader.Domain.Enums;

namespace Trader.Adapter.IndependentReserve;

public static class CodeConverter {

    public static CurrencyCode GetCurrencyCode(CryptoCurrency code)
    {
        if (code.ToString() == "BTC")
            return CurrencyCode.Xbt;
        if (code.ToString() == "ETH")
            return CurrencyCode.Eth;

        throw new ArgumentException($"Invalid code: {code}");
    }

    public static CurrencyCode GetCurrencyCode(FiatCurrency code)
    {
        if (code.ToString() == "NZD")
            return CurrencyCode.Nzd;
        if (code.ToString() == "USD")
            return CurrencyCode.Usd;

        throw new ArgumentException($"Invalid code: {code}");
    }
}
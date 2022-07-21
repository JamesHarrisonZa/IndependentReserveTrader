namespace Trader.Adapter.IndependentReserve;

public static class CodeConverter
{
    private static Dictionary<CryptoCurrency, CurrencyCode> _cryptoCurrencyCodes = 
        new Dictionary<CryptoCurrency, CurrencyCode>()
        {
            { CryptoCurrency.BTC, CurrencyCode.Xbt},
            { CryptoCurrency.ETH, CurrencyCode.Eth},
        };

    private static Dictionary<FiatCurrency, CurrencyCode> _fiatCurrencyCodes = 
        new Dictionary<FiatCurrency, CurrencyCode>()
        {
            { FiatCurrency.NZD, CurrencyCode.Nzd},
            { FiatCurrency.USD, CurrencyCode.Usd},
        };

    public static CurrencyCode GetCurrencyCode(CryptoCurrency code)
    {
        return _cryptoCurrencyCodes[code];
    }

    public static CurrencyCode GetCurrencyCode(FiatCurrency code)
    {
        return _fiatCurrencyCodes[code];
    }
}
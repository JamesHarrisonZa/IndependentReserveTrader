namespace Trader.Adapter.IndependentReserve.Tests;

public class CodeConverterTests
{
      [Theory]
      [InlineData(CryptoCurrency.BTC, CurrencyCode.Xbt)]
      [InlineData(CryptoCurrency.ETH, CurrencyCode.Eth)]
      public void Given_CryptoCurrencyCode_When_GetCurrencyCode_Then_Returns_CurrencyCode(
          CryptoCurrency cryptoCurrencyCode, 
          CurrencyCode expectedCurrencyCode
      )
      {
          var actualCurrencyCode = CodeConverter.GetCurrencyCode(cryptoCurrencyCode);

          Assert.Equal(expectedCurrencyCode, actualCurrencyCode);
      }

      [Theory]
      [InlineData(FiatCurrency.NZD, CurrencyCode.Nzd)]
      [InlineData(FiatCurrency.USD, CurrencyCode.Usd)]
      public void Given_FiatCurrencyCode_When_GetCurrencyCode_Then_Returns_CurrencyCode(
          FiatCurrency fiatCurrencyCode, 
          CurrencyCode expectedCurrencyCode
      )
      {
          var actualCurrencyCode = CodeConverter.GetCurrencyCode(fiatCurrencyCode);

          Assert.Equal(expectedCurrencyCode, actualCurrencyCode);
      }
}
using System.Runtime.Serialization;

namespace Trader.Domain.Enums;

public enum FiatCurrency {

  [EnumMember(Value = "NZD")]
  NZD,

  [EnumMember(Value = "USD")]
  USD
}
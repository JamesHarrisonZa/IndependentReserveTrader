using System.Runtime.Serialization;

namespace Trader.Domain.Enums;

public enum CryptoCurrency {

  [EnumMember(Value = "BTC")]
  BTC,

  [EnumMember(Value = "ETH")]
  ETH
}
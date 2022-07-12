namespace Trader.Adapter.IndependentReserve.Repositories;

public class BalancesRepository : IBalancesRepository
{
    private readonly IMarketRepository _marketRepository;
    private readonly IClient _client;
    private readonly MyMemoryCache _memoryCache;

    public BalancesRepository(IMarketRepository marketRepository, IClient client, MyMemoryCache cache)
    {
        _marketRepository = marketRepository;
        _client = client;
        _memoryCache = cache;
    }

    public async Task<decimal> GetBalance(CryptoCurrency cryptoCurrency)
    {
      var currencyCode = CodeConverter.GetCurrencyCode(cryptoCurrency);
      var accounts = await GetAccounts();
      var currencyCodeAccount = accounts.FirstOrDefault(a => a.CurrencyCode == currencyCode);

      return currencyCodeAccount?.AvailableBalance ?? 0;
    }

    public async Task<decimal> GetBalanceValue(CryptoCurrency cryptoCurrency, FiatCurrency fiatCurrency)
    {
        var balance = await GetBalance(cryptoCurrency);
        var currentPrice = await _marketRepository.GetLastMarketPrice(cryptoCurrency, fiatCurrency);

        return Math.Round(balance * currentPrice, 2);
    }

    private async Task<IEnumerable<Account>> GetAccounts()
    {
        var cachedAccounts = _memoryCache.TryGetCacheValue<IEnumerable<Account>>(CacheKeys.Accounts);

        if (_memoryCache.IsCached(cachedAccounts))
            return cachedAccounts;

        var accounts = await _client.GetAccountsAsync();
        _memoryCache.AddToCache(accounts);
        return accounts;
    }
}
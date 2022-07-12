namespace Trader.Adapter.IndependentReserve.Cache;

public class MyMemoryCache
{
    private IMemoryCache _memoryCache;

    public MyMemoryCache()
    {
        var options = new MemoryCacheOptions();
        _memoryCache = new MemoryCache(options);
    }

    public T TryGetCacheValue<T>(string key)
    {
        _memoryCache.TryGetValue(key, out T value);
        return value;
    }

    public bool IsCached<T>(T value)
    {
        return value != null;
    }

    public void AddToCache<T>(T value)
    {
        var twoMinutes = 2;
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(twoMinutes));

        _memoryCache.Set(CacheKeys.Accounts, value, cacheEntryOptions);
    }
}
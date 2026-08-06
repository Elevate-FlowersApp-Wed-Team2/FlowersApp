using StackExchange.Redis;

namespace FlowersApp.Shared.Redis;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _db;

    public RedisCacheService(IConnectionMultiplexer mux)
    {
        _db = mux.GetDatabase();
    }

    public async Task<string?> GetAsync(string key)
    {
        var value = await _db.StringGetAsync(key);
        return value.IsNullOrEmpty ? null : value.ToString();
    }


    public Task SetAsync(string key, string value, TimeSpan? expiry = null)
    {
        if (expiry.HasValue)
            return _db.StringSetAsync(key, value, expiry.Value);

        return _db.StringSetAsync(key, value);
    }
    public Task<bool> DeleteAsync(string key) => _db.KeyDeleteAsync(key);

    public Task<bool> ExistsAsync(string key) => _db.KeyExistsAsync(key);

    public Task<long> IncrementAsync(string key) => _db.StringIncrementAsync(key);

    public Task<bool> ExpireAsync(string key, TimeSpan expiry) => _db.KeyExpireAsync(key, expiry);

    
}
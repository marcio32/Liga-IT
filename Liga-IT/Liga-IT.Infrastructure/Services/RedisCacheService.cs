using Liga_IT.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

using System.Text.Json;
namespace Liga_IT.Infrastructure.Services;

internal class RedisCacheService(IDistributedCache cache) : IRedisCacheService
{
    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await cache.GetStringAsync(key);
        return value is null ? default : JsonSerializer.Deserialize<T>(value);
    }

    public async Task RemoveAsync(string key)
    {
        await cache.RemoveAsync(key);
    }

    public async Task RemovePatternAsync(string pattern)
    {
        if(pattern == "clubs:*")
        {
            await RemoveAsync("clubs:all");
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new DistributedCacheEntryOptions();

        if (expiration.HasValue)
            options.SetAbsoluteExpiration(expiration.Value);
        else
            options.SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

        await cache.SetStringAsync(key, JsonSerializer.Serialize(value), options);
    }
}

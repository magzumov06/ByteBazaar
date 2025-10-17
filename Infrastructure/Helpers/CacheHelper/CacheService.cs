using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Helpers.CacheHelper;

public class CacheService(IDistributedCache cache): ICacheService
{
    public async Task<T?> GetAsync<T>(string key)
    {
        var res = await cache.GetAsync(key);
        if (res != null)
        {
            return JsonSerializer.Deserialize<T>(res);
        }
        return default;
    }
    
    public async Task AddAsync<T>(string key, T entity, DateTimeOffset expirationTime)
    {
        var jsonSerializerOption = new JsonSerializerOptions() { WriteIndented = true };
        var jsonObject = JsonSerializer.Serialize(entity, jsonSerializerOption);
        var cacheOption = new DistributedCacheEntryOptions() { AbsoluteExpiration = expirationTime };
        await cache.SetStringAsync(key, jsonObject, cacheOption);

    }

    public async Task RemoveAsync(string key)
    {
        await cache.RemoveAsync(key);
    }
}
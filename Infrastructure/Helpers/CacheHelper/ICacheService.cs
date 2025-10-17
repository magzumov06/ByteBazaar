namespace Infrastructure.Helpers.CacheHelper;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task AddAsync<T>(string key, T value, DateTimeOffset expirationTime );
    Task RemoveAsync(string key);
}
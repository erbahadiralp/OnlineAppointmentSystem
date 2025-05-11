using Microsoft.Extensions.Caching.Distributed;
using OnlineAppointmentSystem.DataAccess.Cache;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.DataAccess.Cache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public RedisCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public T Get<T>(string key)
        {
            var value = _distributedCache.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _distributedCache.GetStringAsync(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        public void Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            var options = new DistributedCacheEntryOptions();

            if (expiry.HasValue)
            {
                options.SetAbsoluteExpiration(expiry.Value);
            }
            else
            {
                options.SetAbsoluteExpiration(TimeSpan.FromHours(1)); // Default 1 saat
            }

            var serializedValue = JsonSerializer.Serialize(value);
            _distributedCache.SetString(key, serializedValue, options);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var options = new DistributedCacheEntryOptions();

            if (expiry.HasValue)
            {
                options.SetAbsoluteExpiration(expiry.Value);
            }
            else
            {
                options.SetAbsoluteExpiration(TimeSpan.FromHours(1)); // Default 1 saat
            }

            var serializedValue = JsonSerializer.Serialize(value);
            await _distributedCache.SetStringAsync(key, serializedValue, options);
        }

        public void Remove(string key)
        {
            _distributedCache.Remove(key);
        }

        public async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        public bool Exists(string key)
        {
            return _distributedCache.GetString(key) != null;
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _distributedCache.GetStringAsync(key) != null;
        }
    }
}
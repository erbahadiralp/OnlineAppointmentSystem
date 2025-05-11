using Microsoft.Extensions.Caching.Memory;
using OnlineAppointmentSystem.DataAccess.Cache;
using System;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.DataAccess.Cache
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get<T>(string key)
        {
            return _memoryCache.TryGetValue(key, out T value) ? value : default;
        }

        public Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult(Get<T>(key));
        }

        public void Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            var options = new MemoryCacheEntryOptions();

            if (expiry.HasValue)
            {
                options.SetAbsoluteExpiration(expiry.Value);
            }
            else
            {
                options.SetAbsoluteExpiration(TimeSpan.FromHours(1)); // Default 1 saat
            }

            _memoryCache.Set(key, value, options);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            Set(key, value, expiry);
            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public Task RemoveAsync(string key)
        {
            Remove(key);
            return Task.CompletedTask;
        }

        public bool Exists(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        public Task<bool> ExistsAsync(string key)
        {
            return Task.FromResult(Exists(key));
        }
    }
}
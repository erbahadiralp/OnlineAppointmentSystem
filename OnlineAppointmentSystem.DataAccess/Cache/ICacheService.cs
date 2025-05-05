using System;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.DataAccess.Cache
{
    public interface ICacheService
    {
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        void Set<T>(string key, T value, TimeSpan? expiry = null);
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
        void Remove(string key);
        Task RemoveAsync(string key);
        bool Exists(string key);
        Task<bool> ExistsAsync(string key);
    }
}
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowersApp.Shared.Redis
{
    public interface IRedisCacheService
    {
        Task<string?> GetAsync(string key);
        Task SetAsync(string key, string value, TimeSpan? expiry = null);
        Task<bool> DeleteAsync(string key);
        Task<bool> ExistsAsync(string key);
        Task<long> IncrementAsync(string key);
        Task<bool> ExpireAsync(string key, TimeSpan expiry);
    }
}

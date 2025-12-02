using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Abstraction.Services;
using ECommerce.Domain.Contracts;

namespace ECommerce.Service.Services
{
    public class CacheServices(ICacheRepository cacheRepository) : ICacheServices
    {
        public async Task<string?> GetAsync(string CacheKey)
        {
            return await cacheRepository.GetAsync(CacheKey);
        }

        public async Task SetAsync(string CacheKey, object CacheValue, TimeSpan TimeToLive)
        {
            var Value = JsonSerializer.Serialize(CacheValue);
            await cacheRepository.SetAsync(CacheKey, Value, TimeToLive);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using StackExchange.Redis;

namespace ECommerce.Persistence.Repos
{
    public class CacheRepository(IConnectionMultiplexer connection) : ICacheRepository
    {
        private IDatabase _database = connection.GetDatabase();
        public async Task<string?> GetAsync(string CacheKey)
        {
            var CacheValue = await _database.StringGetAsync(CacheKey);
            return CacheValue.IsNullOrEmpty ? null : CacheValue.ToString();
        }

        public async Task SetAsync(string CacheKey, string CacheValue, TimeSpan TimeToLive)
        {
            await _database.StringSetAsync(CacheKey, CacheValue, TimeToLive);
        }
    }
}

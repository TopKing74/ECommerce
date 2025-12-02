using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts
{
    public interface ICacheRepository
    {
        //Get
        Task<string?> GetAsync(string key);
        //Set
        Task SetAsync(string CacheKey, string CacheValue, TimeSpan TimeToLive);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Models.Baskets;
using StackExchange.Redis;

namespace ECommerce.Persistence.Repos
{
    public class BasketRepository(IConnectionMultiplexer connection) : IBasketRepository
    {
        private readonly IDatabase _database = connection.GetDatabase();
        public async Task<CustomerBasket?> CreateUpdateBasketAsync(CustomerBasket basket, TimeSpan? TimeToLive = null)
        {
            var JsonBasket = JsonSerializer.Serialize(basket);
            var IsCreatedOrUpdated = await _database.StringSetAsync(basket.Id, JsonBasket, TimeToLive ?? TimeSpan.FromHours(5));
            if (IsCreatedOrUpdated)
                return await GetBasketAsync(basket.Id);
            else
                return null;
        }

        public async Task<bool> DeleteBasketAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string key)
        {
            var Basket = await _database.StringGetAsync(key);
            if (Basket.IsNullOrEmpty)
                return null;

            else
                return JsonSerializer.Deserialize<CustomerBasket>(Basket);
        }
    }
}

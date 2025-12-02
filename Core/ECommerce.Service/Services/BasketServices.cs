using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Abstraction.Services;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.Models.Baskets;
using ECommerce.Shared.Dtos.BasketDto_s;

namespace ECommerce.Service.Services
{
    public class BasketServices(IBasketRepository repository, IMapper mapper) : IBasketService
    {
        public async Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basket)
        {
            var CustomerBasket = mapper.Map<BasketDto, CustomerBasket>(basket);
            var SavedBasket = await repository.CreateUpdateBasketAsync(CustomerBasket);
            if (SavedBasket is not null)
                return await GetBasketAsync(SavedBasket.Id);
            else
                throw new Exception("Something Went Wrong at Process");
        }

        public async Task<bool> DeleteBasketAsync(string key)
        {
            return await repository.DeleteBasketAsync(key);
        }

        public async Task<BasketDto> GetBasketAsync(string key)
        {
            var Basket = await repository.GetBasketAsync(key);
            if (Basket is not null)
                return mapper.Map<CustomerBasket, BasketDto>(Basket);
            else
                throw new BasketNotFoundException(key);
        }
    }
}

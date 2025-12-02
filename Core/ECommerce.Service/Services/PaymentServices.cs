using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Abstraction.Services;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Contracts.UOW;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.Models.Baskets;
using ECommerce.Domain.Models.Orders;
using ECommerce.Domain.Models.Products;
using ECommerce.Shared.Dtos.BasketDto_s;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace ECommerce.Service.Services
{
    public class PaymentServices(IConfiguration configuration,IMapper mapper,IBasketRepository basketRepository,IUnitOfWork unitOfWork) : IPaymentServices
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string BasketId)
        {
            //Configre Stripe : Install Stripe.NET
            StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];
            //GetBasket By BasketId
            var Basket = await basketRepository.GetBasketAsync(BasketId) ?? throw new BasketNotFoundException(BasketId);
            //Get Amount - GetProduct + DelveryMethod Cost
            var ProductRepo = unitOfWork.GetRepository<Domain.Models.Products.Product, int>();
            foreach (var item in Basket.Items)
            {
                var Product = await ProductRepo.GetByIdAsync(item.Id) ?? throw new ProductNotFound(item.Id);
                item.Price = Product.Price;
            }
            var DeliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(Basket.DeliveryMethodId.Value)
                ?? throw new DeliveryMethodNotFoundException(Basket.DeliveryMethodId.Value);

            Basket.ShippingPrice = DeliveryMethod.Price;

            var BasketAmount = (long)(Basket.Items.Sum(item => item.Quantity * item.Price) + DeliveryMethod.Price) * 100;
            //Create Payment Intent [Create-Update]
            var PaymentService = new PaymentIntentService();

            if (Basket.PaymentIntentId is null)
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = BasketAmount,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };
                var PaymentIntent = await PaymentService.CreateAsync(Options);
                Basket.PaymentIntentId = PaymentIntent.Id;
                Basket.ClientSecret = PaymentIntent.ClientSecret;
            }
            else
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = BasketAmount
                };
                await PaymentService.UpdateAsync(Basket.PaymentIntentId, Options);
            }
            await basketRepository.CreateUpdateBasketAsync(Basket);
            return mapper.Map<CustomerBasket,BasketDto>(Basket);
        }
    }
}

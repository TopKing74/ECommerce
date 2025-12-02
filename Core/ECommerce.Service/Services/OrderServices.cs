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
using ECommerce.Domain.Models.Orders;
using ECommerce.Domain.Models.Products;
using ECommerce.Service.Specifications;
using ECommerce.Shared.Dtos.IdentityDto_s;
using ECommerce.Shared.Dtos.OrderDto_s;

namespace ECommerce.Service.Services
{
    public class OrderServices(IMapper mapper,IBasketRepository basketRepository,IUnitOfWork unitOfWork) : IOrderServices
    {
        public async Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string Email)
        {
            //Map Address To Order Address
            var OrderAddress = mapper.Map<AddressDto, OrderAddress>(orderDto.Address);
            //Get Basket
            var Basket = await basketRepository.GetBasketAsync(orderDto.BasketId) ?? throw new BasketNotFoundException(orderDto.BasketId);

            ArgumentNullException.ThrowIfNullOrEmpty(Basket.PaymentIntentId);
            var OrderRepo = unitOfWork.GetRepository<Order, Guid>();
            var Specs = new OrderWithPaymentIntentIdSpecifications(Basket.PaymentIntentId);
            var ExistOrder = await OrderRepo.GetByIdWithSpecAsync(Specs);
            if (ExistOrder is not null) OrderRepo.Delete(ExistOrder);

            //Create OrderItems List
            List<OrderItem> OrderItems = [];

            var ProductRepo = unitOfWork.GetRepository<Product, int>();
            foreach (var Item in Basket.Items)
            {
                var Product = await ProductRepo.GetByIdAsync(Item.Id) ?? throw new ProductNotFound(Item.Id);
                var OrderItem = new OrderItem()
                {
                    Product = new ProductItemOrdered()
                    {
                        ProductId = Product.Id,
                        ProductName = Product.Name,
                        PictureUrl = Product.PictureUrl
                    },
                    Quantity = Item.Quantity,
                    Price = Product.Price
                };

                OrderItems.Add(OrderItem);
            }

            //Get Deliveery Method
            var DeliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId) ?? throw new DeliveryMethodNotFoundException(orderDto.DeliveryMethodId);

            //SubTotal
            var SubTotal = OrderItems.Sum(I => I.Price * I.Quantity);

            var Order = new Order(Email, OrderAddress, DeliveryMethod, OrderItems, SubTotal,Basket.PaymentIntentId);

            unitOfWork.GetRepository<Order, Guid>().Add(Order);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<Order, OrderToReturnDto>(Order);
        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethodsAsync()
        {
            var DeliveryMethods = await unitOfWork.GetRepository<DeliveryMethod,int>().GetAllAsync();
            return mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMethodDto>>(DeliveryMethods);
        }

        public async Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync(string Email)
        {
            var Spec = new OrderSpecifications(Email);
            var Orders = await unitOfWork.GetRepository<Order, Guid>().GetAllWithSpecAsync(Spec);

            return mapper.Map<IEnumerable<Order>, IEnumerable<OrderToReturnDto>>(Orders);
        }

        public async Task<OrderToReturnDto> GetOrderByIdAsync(Guid OrderId)
        {
            var Spec = new OrderSpecifications(OrderId);
            var Order = await unitOfWork.GetRepository<Order, Guid>().GetByIdWithSpecAsync(Spec);
            return mapper.Map<Order, OrderToReturnDto>(Order);
        }
    }
}

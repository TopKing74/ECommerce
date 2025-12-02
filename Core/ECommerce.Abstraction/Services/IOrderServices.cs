using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Shared.Dtos.OrderDto_s;

namespace ECommerce.Abstraction.Services
{
    public interface IOrderServices
    {
        //Create order
        //OrderDto [Address - DeliveryMethodId - BasketId] || UserEmail
        //OrderToReturnDto [Id,Email,OrderDate,Items,Address,DeliveryMethod,OrderState,Subtotal,Total Price]

        Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string Email);

        //Get All Delivery Methods
        Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethodsAsync();
        //Get All Orders For CurrentUser
        Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync(string Email);
        //Get Specific Order For CurrentUser
        Task<OrderToReturnDto> GetOrderByIdAsync(Guid OrderId);
    }
}

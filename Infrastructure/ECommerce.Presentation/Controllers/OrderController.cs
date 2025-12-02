using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Abstraction.Services;
using ECommerce.Shared.Dtos.OrderDto_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OrderController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Order = await serviceManager.OrderServices.CreateOrderAsync(orderDto, Email);
            return Ok(Order);
        }
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetDeliveryMethods()
        {
            var DeliveryMethods = await serviceManager.OrderServices.GetAllDeliveryMethodsAsync();
            return Ok(DeliveryMethods);
        }
        [HttpGet("AllOrders")]
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetAllOrdersForUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await serviceManager.OrderServices.GetAllOrdersAsync(Email);
            return Ok(Orders);
        }
        [HttpGet]
        public async Task<ActionResult<OrderToReturnDto>> GetSpecificOrder(Guid OrderId)
        {
            var Order = await serviceManager.OrderServices.GetOrderByIdAsync(OrderId);
            return Ok(Order);
        }
    }
}

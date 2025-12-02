using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Shared.Dtos.IdentityDto_s;

namespace ECommerce.Shared.Dtos.OrderDto_s
{
    public class OrderToReturnDto
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; }
        public AddressDto Address { get; set; } = null!;
        public string DeliveryMethod { get; set; } = null!;
        public string OrderState { get; set; } = null!;
        public ICollection<OrderItemDto> Items { get; set; } = [];
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }
}

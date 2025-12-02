using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Models.Orders
{
    public class OrderItem:BaseEntity<int>
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ProductItemOrdered Product { get; set; }
    }
}

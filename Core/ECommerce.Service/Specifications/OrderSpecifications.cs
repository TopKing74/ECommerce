using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Models.Orders;

namespace ECommerce.Service.Specifications
{
    public class OrderSpecifications:BaseSpecifications<Order,Guid>
    {
        public OrderSpecifications(string Email):base(O=>O.UserEmail==Email)
        {
            AddInclude(O => O.Items);
            AddInclude(O => O.DeliveryMethod);
            AddOrderByDesc(O => O.OrderDate);
        }
        public OrderSpecifications(Guid Id):base(O=>O.Id==Id)
        {
            AddInclude(O => O.Items);
            AddInclude(O => O.DeliveryMethod);
        }
    }
}

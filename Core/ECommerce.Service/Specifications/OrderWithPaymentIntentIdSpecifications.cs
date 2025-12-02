using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Models.Orders;

namespace ECommerce.Service.Specifications
{
    public class OrderWithPaymentIntentIdSpecifications:BaseSpecifications<Order,Guid>
    {
        public OrderWithPaymentIntentIdSpecifications(string IntentId):base(O=>O.PaymentIntentId== IntentId)
        {
            
        }
    }
}

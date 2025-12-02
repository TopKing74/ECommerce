using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Abstraction.Services
{
    public interface IServiceManager
    {
        public IProductService ProductService { get; }
        public IBasketService BasketService { get; }
        public IAuthenticationServices AuthenticationServices { get; }
        public IOrderServices OrderServices { get; }
        public IPaymentServices PaymentServices { get; }
    }
}

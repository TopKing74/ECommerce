using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Abstraction.Services;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Contracts.UOW;
using ECommerce.Domain.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Service.Services
{
    public class ServiceManager(IMapper mapper,IUnitOfWork unitOfWork,IBasketRepository basketRepository,UserManager<ApplicationUser> userManager,IConfiguration configuration) : IServiceManager
    {
        #region Product
        private readonly Lazy<IProductService> LazyProductServices = new Lazy<IProductService>(() => new ProductServices(unitOfWork, mapper));
        public IProductService ProductService => LazyProductServices.Value;
        #endregion
        #region Basket
        private readonly Lazy<IBasketService> LazyBasketServices = new Lazy<IBasketService>(() => new BasketServices(basketRepository, mapper));
        public IBasketService BasketService => LazyBasketServices.Value;
        #endregion

        #region Authentication
        private readonly Lazy<IAuthenticationServices> LazyAuthenticationServices = new Lazy<IAuthenticationServices>(() => new AuthenticationServices(userManager, configuration, mapper));
        public IAuthenticationServices AuthenticationServices => LazyAuthenticationServices.Value;
        #endregion

        #region Order
        private readonly Lazy<IOrderServices> LazyOrderServices = new Lazy<IOrderServices>(() => new OrderServices(mapper, basketRepository, unitOfWork));
        public IOrderServices OrderServices => LazyOrderServices.Value;

        private readonly Lazy<IPaymentServices> LazyPaymentServices = new Lazy<IPaymentServices>(() => new PaymentServices(configuration,mapper, basketRepository, unitOfWork));
        public IPaymentServices PaymentServices => LazyPaymentServices.Value;
        #endregion
    }
}

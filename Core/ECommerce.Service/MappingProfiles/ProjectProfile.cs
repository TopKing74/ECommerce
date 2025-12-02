using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Domain.Models.Baskets;
using ECommerce.Domain.Models.Orders;
using ECommerce.Domain.Models.Products;
using ECommerce.Shared.Dtos;
using ECommerce.Shared.Dtos.BasketDto_s;
using ECommerce.Shared.Dtos.IdentityDto_s;
using ECommerce.Shared.Dtos.OrderDto_s;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Service.MappingProfiles
{
    public class ProjectProfile:Profile
    {
        public ProjectProfile(IConfiguration configuration)
        {
            #region Product
            CreateMap<Product, ProductDto>()
                    .ForMember(dest => dest.BrandName, options => options.MapFrom(src => src.Brand.Name))
                    .ForMember(dest => dest.TypeName, options => options.MapFrom(src => src.Type.Name))
                    .ForMember(dest => dest.PictureUrl, options => options.MapFrom(new PictureUrlResolver(configuration)));
            CreateMap<ProductBrand, BrandDto>();
            CreateMap<ProductType, TypeDto>();
            #endregion
            #region Basket
            CreateMap<CustomerBasket, BasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();

            #endregion
            #region Order
            CreateMap<AddressDto, OrderAddress>().ReverseMap();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(D => D.DeliveryMethod, options => options.MapFrom(src => src.DeliveryMethod.ShortName));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(src => src.ProductName, options => options.MapFrom(src => src.Product.ProductName))
                .ForMember(src => src.PictureUrl, options => options.MapFrom(new OrderPictureUrlResolver(configuration)));

            CreateMap<DeliveryMethod, DeliveryMethodDto>().ReverseMap();
            #endregion
        }
    }
}

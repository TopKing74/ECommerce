using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Models.Products;
using ECommerce.Shared.Common;

namespace ECommerce.Service.Specifications
{
    public class CountProductSpecifications:BaseSpecifications<Product, int>
    {
        public CountProductSpecifications(ProductQueryParams productQueryParams) : base(P => (!productQueryParams.BrandId.HasValue || P.BrandId == productQueryParams.BrandId) && (!productQueryParams.TypeId.HasValue || P.TypeId == productQueryParams.TypeId) && (string.IsNullOrEmpty(productQueryParams.SearchValue) || P.Name.ToLower().Contains(productQueryParams.SearchValue.ToLower())))
        {

        }
    }
}

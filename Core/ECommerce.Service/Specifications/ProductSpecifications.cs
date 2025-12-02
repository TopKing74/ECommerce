using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Models.Products;
using ECommerce.Shared.Common;

namespace ECommerce.Service.Specifications
{
    public class ProductSpecifications:BaseSpecifications<Product, int>
    {
        public ProductSpecifications(ProductQueryParams productQueryParams) : base(P=>(!productQueryParams.BrandId.HasValue || P.BrandId== productQueryParams.BrandId)&&(!productQueryParams.TypeId.HasValue || P.TypeId == productQueryParams.TypeId)&&(string.IsNullOrEmpty(productQueryParams.SearchValue)||P.Name.ToLower().Contains(productQueryParams.SearchValue.ToLower())))
        {
            AddInclude(P => P.Brand);
            AddInclude(P => P.Type);

            switch (productQueryParams.SortingOptions)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(P => P.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDesc(P => P.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(P => P.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDesc(P => P.Price);
                    break;
                default:
                    break;
            }
            ApplyPagination(productQueryParams.PageIndex, productQueryParams.PageSize);
        }
        public ProductSpecifications(int id) : base(P=>P.Id == id)
        {
            AddInclude(P => P.Brand);
            AddInclude(P => P.Type);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Shared.Common;
using ECommerce.Shared.Dtos;

namespace ECommerce.Abstraction.Services
{
    public interface IProductService
    {
        Task<PaginationResult<ProductDto>> GetAllProductAsync(ProductQueryParams productQueryParams);
        Task<IEnumerable<TypeDto>> GetAllTypesAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<IEnumerable<BrandDto>> GetAllBrandsAsync(); // Added missing method

    }
}

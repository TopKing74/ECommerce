using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Abstraction.Services;
using ECommerce.Domain.Contracts.UOW;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.Models.Products;
using ECommerce.Service.Specifications;
using ECommerce.Shared.Common;
using ECommerce.Shared.Dtos;

namespace ECommerce.Service.Services
{
    public class ProductServices(IUnitOfWork UnitOfWork,IMapper mapper) : IProductService
    {
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            var Repo = UnitOfWork.GetRepository<ProductBrand,int>();
            var Brands = await Repo.GetAllAsync();
            var BrandDto = mapper.Map<IEnumerable<ProductBrand>, IEnumerable<BrandDto>>(Brands);
            return BrandDto;
        }

        public async Task<PaginationResult<ProductDto>> GetAllProductAsync(ProductQueryParams productQueryParams)
        {
            var Spec = new ProductSpecifications(productQueryParams);
            var Products = await UnitOfWork.GetRepository<Product,int>().GetAllWithSpecAsync(Spec);
            var Data = mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(Products);
            var Size = Data.Count();
            //Total Count
            var CountSpec = new CountProductSpecifications(productQueryParams);
            var Count = await UnitOfWork.GetRepository<Product,int>().GetCountWithSpecAsync(CountSpec);
            return new PaginationResult<ProductDto>(productQueryParams.PageIndex, Size, Count, Data);
        }

        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
            var Types = await UnitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            return mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(Types);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var Spec = new ProductSpecifications(id);
            var Product = await UnitOfWork.GetRepository<Product, int>().GetByIdWithSpecAsync(Spec);
            if (Product is null)
            {
                throw new ProductNotFound(id);
            }
            return mapper.Map<Product,ProductDto>(Product);
        }
    }
}

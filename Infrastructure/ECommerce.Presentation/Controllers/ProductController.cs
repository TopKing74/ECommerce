using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Abstraction.Services;
using ECommerce.Presentation.CacheAttributes;
using ECommerce.Shared.Common;
using ECommerce.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProductController(IServiceManager serviceManager): ControllerBase
    {
        //Get All Products
        //Get BaseURL/api/product
        [HttpGet]
        [Cache]
        public async Task<ActionResult<PaginationResult<ProductDto>>> GetAllProducts([FromQuery]ProductQueryParams productQueryParams)
        {
            var Products = await serviceManager.ProductService.GetAllProductAsync(productQueryParams);
            return Ok(Products);//status code 200
        }

        [HttpGet("Brands")]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllBrands()
        {
            var Brands = await serviceManager.ProductService.GetAllBrandsAsync();
            return Ok(Brands);//status code 200
        }

        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetAllTypes()
        {
            var Types = await serviceManager.ProductService.GetAllTypesAsync();
            return Ok(Types);//status code 200
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var Product = await serviceManager.ProductService.GetProductByIdAsync(id);
            return Ok(Product);//status code 200
        }
    }
}

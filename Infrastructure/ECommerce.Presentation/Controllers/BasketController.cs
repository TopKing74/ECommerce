using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Abstraction.Services;
using ECommerce.Shared.Dtos.BasketDto_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class BasketController(IServiceManager serviceManager):Controller
    {
        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasket(string key)
        {
            var Basket = await serviceManager.BasketService.GetBasketAsync(key);
            return Ok(Basket);
        }
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateUpdateBasket(BasketDto basket)
        {
            var InternalBasket = await serviceManager.BasketService.CreateOrUpdateBasketAsync(basket);
            return Ok(InternalBasket);
        }
        [HttpDelete("{key}")]
        public async Task<ActionResult<bool>> DeleteBasket(string key)
        {
            var Result = await serviceManager.BasketService.DeleteBasketAsync(key);
            return Ok(Result);
        }
    }
}

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
    [Route("api/[controller]")]
    public class PaymentController(IServiceManager serviceManager) : ControllerBase
    {
        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string BasketId)
        {
            var Basket = await serviceManager.PaymentServices.CreateOrUpdatePaymentIntentAsync(BasketId);
            
            return Ok(Basket);
        }
    }
}

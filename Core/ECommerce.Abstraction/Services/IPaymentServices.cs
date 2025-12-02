using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Shared.Dtos.BasketDto_s;

namespace ECommerce.Abstraction.Services
{
    public interface IPaymentServices
    {
        Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string BasketId);
    }
}

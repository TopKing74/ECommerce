using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Exceptions
{
    public class DeliveryMethodNotFoundException(int Id):NotFoundException($"DeliveryMehod with id {Id} not Found!!")
    {
    }
}

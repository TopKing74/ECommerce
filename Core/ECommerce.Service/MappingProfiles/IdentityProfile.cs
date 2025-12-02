using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Domain.Identity.Models;
using ECommerce.Shared.Dtos.IdentityDto_s;

namespace ECommerce.Service.MappingProfiles
{
    internal class IdentityProfile:Profile
    {
        public IdentityProfile()
        {
            CreateMap<Address,AddressDto>().ReverseMap();
        }
    }
}

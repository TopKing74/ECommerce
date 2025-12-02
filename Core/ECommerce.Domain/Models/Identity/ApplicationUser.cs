using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Domain.Identity.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string DisplayName { get; set; } = null!;
        public Address Address { get; set; }
    }
}

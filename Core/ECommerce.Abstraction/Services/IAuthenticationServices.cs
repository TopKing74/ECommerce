using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Shared.Dtos.IdentityDto_s;

namespace ECommerce.Abstraction.Services
{
    public interface IAuthenticationServices
    {
        //Login
        //Take Email , Password Then Return Token , Email , DisplayName
        Task<UserDto> LoginAsync(LoginDto dto);
        //Register
        //Take Email , Password , DisplayName , UserName , PhoneNumber Then Return Token , Email , DisplayName
        Task<UserDto> RegisterAsync(RegisterDto dto);
        Task<bool> CheckEmailAsync(string email);
        Task<UserDto> GetCurrentUserAsync(string email);
        Task<AddressDto> GetCurrentUserAddressAsync(string email);
        Task<AddressDto> UpdateUserAddressAsync(string email, AddressDto addressDto);
    }
}

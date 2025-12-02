using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Abstraction.Services;
using ECommerce.Shared.Dtos.IdentityDto_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController(IServiceManager servicesManager) : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto dto)
        {
            var User = await servicesManager.AuthenticationServices.LoginAsync(dto);
            return Ok(User);
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto dto)
        {
            var User = await servicesManager.AuthenticationServices.RegisterAsync(dto);
            return Ok(User);
        }
        [Authorize]
        [HttpGet("CheckEmail")]
        public async Task<ActionResult<bool>> CheckEmail(string Email)
        {
            var Result = await servicesManager.AuthenticationServices.CheckEmailAsync(Email);
            return Ok(Result);
        }
        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var AppUser = await servicesManager.AuthenticationServices.GetCurrentUserAsync(Email);
            return Ok(AppUser);
        }
        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Address = await servicesManager.AuthenticationServices.GetCurrentUserAddressAsync(Email);
            return Ok(Address);
        }
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto addressDto)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var UpdatedAddress = await servicesManager.AuthenticationServices.UpdateUserAddressAsync(Email, addressDto);
            return Ok(UpdatedAddress);
        }
    }
}

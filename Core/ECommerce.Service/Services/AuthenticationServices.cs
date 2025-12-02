using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Abstraction.Services;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.Identity.Models;
using ECommerce.Shared.Dtos.IdentityDto_s;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Service.Services
{
    public class AuthenticationServices(UserManager<ApplicationUser> userManager,IConfiguration configuration,IMapper mapper) : IAuthenticationServices
    {
        public async Task<UserDto> LoginAsync(LoginDto dto)
        {
            var User = await userManager.FindByEmailAsync(dto.Email) ?? throw new UserNotFoundException(dto.Email);

            var IsPasswordValid = await userManager.CheckPasswordAsync(User, dto.Password);
            if (IsPasswordValid)
            {
                return new UserDto()
                {
                    Email = User.Email,
                    DisplayName = User.DisplayName,
                    Token = await CreateTokenAsync(User)
                };
            }
            else
            {
                throw new UnAuthorizedException();
            }
        }

        public async Task<UserDto> RegisterAsync(RegisterDto dto)
        {
            var User = new ApplicationUser()
            {
                DisplayName = dto.DisplayName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                UserName = dto.UserName,
            };

            var Result = await userManager.CreateAsync(User, dto.Password);

            if (Result.Succeeded)
            {
                return new UserDto()
                {
                    Email = User.Email,
                    DisplayName = User.DisplayName,
                    Token = await CreateTokenAsync(User)
                };
            }
            else
            {
                var Errors = Result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(Errors);
            }
        }
        public async Task<bool> CheckEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user != null;
        }
        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var UserClaims = new List<Claim>
            {
                new(ClaimTypes.Email,user.Email),
                new(ClaimTypes.Name,user.UserName),
                new(ClaimTypes.NameIdentifier,user.Id)
            };
            var Roles = await userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                UserClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var SecurityKey = configuration.GetSection("JWTOptions")["SecurityKey"];
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));
            var Creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            var Token = new JwtSecurityToken
            (
                issuer: configuration.GetSection("JWTOptions")["issuer"],
                audience: configuration.GetSection("JWTOptions")["audience"],
                claims: UserClaims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: Creds
            );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

        public async Task<UserDto> GetCurrentUserAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) throw new UserNotFoundException(email);
            return new UserDto
            {
                Email = email,
                DisplayName = user.DisplayName,
                Token = await CreateTokenAsync(user)
            };
        }

        public async Task<AddressDto> GetCurrentUserAddressAsync(string email)
        {
            var user = await userManager.Users.Include(u => u.Address)
                                                .FirstOrDefaultAsync(u => u.Email == email) ?? throw new UserNotFoundException(email);
            if (user.Address is not null)
                return mapper.Map<Address,AddressDto>(user.Address);
            else
                throw new AddressNotFoundException(user.UserName);
        }

        public async Task<AddressDto> UpdateUserAddressAsync(string email, AddressDto addressDto)
        {
            var user = await userManager.Users.Include(u => u.Address)
                                                .FirstOrDefaultAsync(u => u.Email == email) ?? throw new UserNotFoundException(email);
            if (user.Address is null)
            {
                user.Address.FirstName = addressDto.FirstName;
                user.Address.LastName = addressDto.LastName;
                user.Address.Street = addressDto.Street;
                user.Address.City = addressDto.City;
                user.Address.Country = addressDto.Country;
            }
            else
            {
                var NewAddress = mapper.Map<AddressDto, Address>(addressDto);
            }
            await userManager.UpdateAsync(user);
            return mapper.Map<Address, AddressDto>(user.Address);
        }
    }
}

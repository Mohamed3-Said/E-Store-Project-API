using AutoMapper;
using DomainLayer.Exceptions;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared.DTOS.IdentityModuleDTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager 
        ,IConfiguration _configuration , IMapper _mapper) : IAuthenticationService
    {
        public async Task<bool> CheckEmailAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            if(User is null)
                return false;
            else
                return true;
        }

        public async Task<UserDto> GetCurrentUserAsync(string Email)
        {
            var User = await _userManager.FindByEmailAsync(Email) ?? throw new UserNotFoundException(Email);
            return new UserDto() { DisplayName = User.DisplayName, Email = User.Email, Token = await CreateTokenAsync(User) };
        }

        public async Task<AddressDto> GetCurrentUserAddressAsync(string Email)
        {
            var User = await _userManager.Users.Include(U => U.Address)
                                               .FirstOrDefaultAsync(U => U.Email == Email) ?? throw new UserNotFoundException(Email);
            if (User.Address is not null)
            {
                return _mapper.Map<Address, AddressDto>(User.Address);
            }
            else
            {
                throw new AddressNotFoundException(User.UserName!);

            }
        }
        public async Task<AddressDto> UpdateCurrentUserAddressAsync(string Email, AddressDto addressDto)
        {
            var User = await _userManager.Users.Include(U => U.Address)
                                               .FirstOrDefaultAsync(U => U.Email == Email) ?? throw new UserNotFoundException(Email);
            if (User.Address is not null) //Update Address :
            {
                User.Address.FirstName = addressDto.FirstName;
                User.Address.LastName = addressDto.LastName;
                User.Address.Street = addressDto.Street;
                User.Address.City = addressDto.City;
                User.Address.Country = addressDto.Country;
            }
            else //Create New Address:
            {
              User.Address = _mapper.Map<AddressDto, Address>(addressDto);
            }
            await _userManager.UpdateAsync(User);
            return _mapper.Map<Address, AddressDto>(User.Address!);
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            //1-check Email:
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
                throw new UserNotFoundException(loginDto.Email);
            //2-check Password:
            var IsPAsswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (IsPAsswordValid)
            {
                //Return UserDto:
                return new UserDto
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email!,
                    Token = await CreateTokenAsync(user)
                };
            }
            else
                throw new UnauthorizedException();

        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            //Mapping from RegisterDto to ApplicationUser:
            var user = new ApplicationUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber
            };
            //Create User:
            var Result = await _userManager.CreateAsync(user, registerDto.Password);
            if (Result.Succeeded)
            {
                //Return UserDto:
                return new UserDto { DisplayName = user.DisplayName, Email = user.Email, Token = await CreateTokenAsync(user) };

            }
            else
            { //errors from Result and Add to exception:
                var Errors = Result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(Errors);
            }
        }


        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            //1-Create Claims:
            var Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.NameIdentifier,user.Id!),
            };
            var Roles = await _userManager.GetRolesAsync(user);
            foreach (var roles in Roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, roles));
            }
            var SecretKey = _configuration.GetSection("JWTOptions")["SecretKey"];
            //Conver SecretKey fromstring to byte array:
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var Creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            //Create Token Descriptor:
            var Token = new JwtSecurityToken(

                issuer: _configuration["JWTOptions:Issuer"],
                audience: _configuration["JWTOptions:Audience"],
                claims: Claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: Creds
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

    }
}

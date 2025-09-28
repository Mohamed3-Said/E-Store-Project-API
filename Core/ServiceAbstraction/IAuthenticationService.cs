using Shared.DTOS.IdentityModuleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IAuthenticationService
    {
        //Login method
        Task<UserDto> LoginAsync(LoginDto loginDto);
        //Register method
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        //Check Email Exists:
        Task<bool> CheckEmailAsync(string email);
        //Get Current User Address:
        Task<AddressDto> GetCurrentUserAddressAsync(string Email);
        //Update Current User Address:
        Task<AddressDto> UpdateCurrentUserAddressAsync( string Email , AddressDto addressDto);
        //Get Current User:
        Task<UserDto> GetCurrentUserAsync(string Email);
    }
}

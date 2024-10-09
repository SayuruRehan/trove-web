// IT21470004 - BOPITIYA S. R. - Interface for authentication service

using backend.DTOs;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

namespace backend.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> Register(UserRegisterDTO userRegisterDTO);
        Task<UserLoginResponseDTO> Login(UserLoginDTO userLoginDTO);

        Task Logout();

        Task<IdentityResult> UpdateUser(string userId, UserUpdateDTO userUpdateDTO);
    }
}

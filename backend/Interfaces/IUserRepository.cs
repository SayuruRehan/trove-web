using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<User> FindByUsernameAsync(string username);
        Task<bool> CheckPasswordAsync(User user, string password);
    }
}

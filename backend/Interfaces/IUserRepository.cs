using backend.Models; // Ensure you have the correct namespace for User
using Microsoft.AspNetCore.Identity; // Required for IdentityResult
using System; // Required for Guid
using System.Threading.Tasks; // Required for Task

namespace backend.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<User> FindByUsernameAsync(string username);
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByIdAsync(Guid userId); // Ensure this matches your implementation
        Task<bool> CheckPasswordAsync(User user, string password);
        Task Logout();
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}

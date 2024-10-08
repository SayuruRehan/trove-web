// IT21470004 - BOPITIYA S. R. - Interface for User Repository

using backend.Models; 
using Microsoft.AspNetCore.Identity; 
using System; 
using System.Threading.Tasks; 

namespace backend.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<User> FindByUsernameAsync(string username);
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByIdAsync(Guid userId); 
        Task<bool> CheckPasswordAsync(User user, string password);
        Task Logout();
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}

// IT21470004 - BOPITIYA S. R. - Interface for User Repository

using backend.Models; // Ensure you have the correct namespace for User
using Microsoft.AspNetCore.Identity; // Required for IdentityResult
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System; // Required for Guid
using System.Threading.Tasks; // Required for Task

namespace backend.Interfaces
{
    public interface IUserRepository
    {

        // user
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<User> FindByUsernameAsync(string username);
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByIdAsync(string userId); // Ensure this matches your implementation
        Task<bool> CheckPasswordAsync(User user, string password);
        Task Logout();
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();

        Task<IEnumerable<User>> GetAllVendorsAsync();

        Task<IEnumerable<User>> GetUnapprovedUsersAsync();
        Task<IEnumerable<User>> GetUnapprovedVendorsAsync();
    }
}

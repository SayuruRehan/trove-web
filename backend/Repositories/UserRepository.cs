using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<User> FindByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            return result.Succeeded;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        // Corrected method signature to match the interface
        public async Task<User> FindByIdAsync(Guid userId) // Change parameter type to Guid
        {
            return await _userManager.FindByIdAsync(userId.ToString()); // Convert Guid to string
        }
    }
}

// IT21470004 - BOPITIYA S. R. - User Repository

using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMongoCollection<User> _users; // MongoDB collection for users
        private readonly IMongoCollection<Vendor> _vendors; // MongoDB collection for vendors

        public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager, IMongoDatabase database)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _users = database.GetCollection<User>("Users"); // Initialize the Users collection

        }

        // User CRUD Operations
        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password); // Identity handles MongoDB storage
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
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            return result.Succeeded;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user); // Identity handles MongoDB storage
        }

        public async Task<User> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await Task.Run(() => _userManager.Users.ToList());
        }


        public async Task<IEnumerable<User>> GetAllVendorsAsync()
        {
            var filter = Builders<User>.Filter.Eq(u => u.IsApproved, true);
            var roleFilter = Builders<User>.Filter.Eq(u => u.Role, "vendor");

            return await _users.Find(roleFilter).ToListAsync();

        }

        public async Task<IEnumerable<User>> GetUnapprovedUsersAsync()
        {
            // Create a filter for isApproved = false
            var filter = Builders<User>.Filter.Eq(u => u.IsApproved, false);
            var roleFilter = Builders<User>.Filter.Eq(u => u.Role, "user");

            var combinedFilter = Builders<User>.Filter.And(filter, roleFilter);

            return await _users.Find(combinedFilter).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUnapprovedVendorsAsync()
        {
            // Create a filter for isApproved = false
            var filter = Builders<User>.Filter.Eq(u => u.IsApproved, false);
            var roleFilter = Builders<User>.Filter.Eq(u => u.Role, "vendor");

            var combinedFilter = Builders<User>.Filter.And(filter, roleFilter);

            return await _users.Find(combinedFilter).ToListAsync();
        }
    }
}

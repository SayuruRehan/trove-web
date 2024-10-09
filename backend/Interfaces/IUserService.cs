// IT21470004 - BOPITIYA S. R. - Interface for User Service

using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson; // For ObjectId

namespace backend.Interfaces
{
    public interface IUserService
    {
        // User-related methods
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();

        Task<IEnumerable<UserDTO>> GetAllVendorsAsync();

        Task<IdentityResult> ApproveUser(string id);

        // Task<IdentityResult> DeactivateUser(string id);

        Task<IEnumerable<UserDTO>> GetAllUnApprovedUsersAsync();
        Task<IEnumerable<UserDTO>> GetAllUnApprovedVendorsAsync();
    }
}

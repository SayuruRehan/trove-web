// IT21470004 - BOPITIYA S. R. - User Service

using backend.DTOs;
using backend.Interfaces;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Identity;


namespace backend.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly EmailService _emailService;

        public UserService(IUserRepository userRepository, EmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        // Method to get all users
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {

            var users = await _userRepository.GetAllUsersAsync();
            // Map Product to ProductDto
            var userDtos = new List<UserDTO>();
            foreach (var user in users)
            {
                userDtos.Add(new UserDTO
                {
                    Id = user.Id.ToString(),
                    Email = user.Email,
                    Firstname = user.Firstname,
                    IsApproved = user.IsApproved,
                    Lastname = user.Lastname,
                    Phone = user.Phone,
                    Role = user.Role
                });
            }
            return userDtos;

        }

        // Method to get all vendors
        public async Task<IEnumerable<UserDTO>> GetAllVendorsAsync()
        {

            var users = await _userRepository.GetAllVendorsAsync();

            var userDtos = new List<UserDTO>();
            foreach (var user in users)
            {
                userDtos.Add(new UserDTO
                {
                    Id = user.Id.ToString(),
                    Email = user.Email,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    IsApproved = user.IsApproved,
                    Phone = user.Phone,
                    Role = user.Role
                });
            }
            return userDtos;

        }

        // Approve user
        public async Task<IdentityResult> ApproveUser(string id)
        {
            // Find the user by ID
            var user = await _userRepository.FindByIdAsync(id);

            // Check if user exists
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            // Mark the user as approved
            user.IsApproved = true;
            var result = await _userRepository.UpdateUserAsync(user);

            // If the update was successful, send an approval email
            if (result.Succeeded)
            {
                var emailDTO = new EmailDTO
                {
                    ToEmail = user.Email,
                    Subject = "Your Registration Has Been Approved",
                    Body = GenerateApprovalEmailBody(user)
                };

                // Send the approval email
                await SendEmail(emailDTO);
            }

            return result;
        }

        // // Deactivate user
        // public async Task<IdentityResult> DeactivateUser(string id)
        // {
        //     // Find the user by ID
        //     var user = await _userRepository.FindByIdAsync(id);

        //     // Check if user exists
        //     if (user == null)
        //     {
        //         return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        //     }

        //     user.IsApproved = false;

        //     var result = await _userRepository.UpdateUserAsync(user);

        //     if (result.Succeeded)
        //     {
        //         return IdentityResult.Success;
        //     }

        //     return result;
        // }


        // Get all approved users
        public async Task<IEnumerable<UserDTO>> GetAllUnApprovedUsersAsync()
        {
            var users = await _userRepository.GetUnapprovedUsersAsync();
            var userDtos = new List<UserDTO>();
            foreach (var user in users)
            {
                userDtos.Add(new UserDTO
                {
                    Id = user.Id.ToString(),
                    Email = user.Email,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    IsApproved = user.IsApproved,
                    Phone = user.Phone,
                    Role = user.Role
                });
            }
            return userDtos;
        }

        // Get all unapproved vendors
        public async Task<IEnumerable<UserDTO>> GetAllUnApprovedVendorsAsync()
        {
            var users = await _userRepository.GetUnapprovedVendorsAsync();
            var userDtos = new List<UserDTO>();
            foreach (var user in users)
            {
                userDtos.Add(new UserDTO
                {
                    Id = user.Id.ToString(),
                    Email = user.Email,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    IsApproved = user.IsApproved,
                    Phone = user.Phone,
                    Role = user.Role
                });
            }
            return userDtos;
        }
        public async Task SendEmail(EmailDTO emailDTO)
        {
            await _emailService.SendEmailAsync(emailDTO);
        }
        private string GenerateApprovalEmailBody(User user)
        {
            // Common content for all users
            string commonBody = $@"
        <h1>Congratulations {user.Firstname} {user.Lastname},</h1>
        <p>We are pleased to inform you that your registration as a <strong>{user.Role}</strong> has been approved.</p>";

            // If the user is a vendor, include specific message and vendor login link
            if (user.Role == "vendor")
            {
                return commonBody + $@"
            <p>You can now log in using the following link to manage your vendor account:</p>
            <a href='http://localhost:5173/vendor/login'>Vendor Login</a>
            <p>If you have any questions, feel free to reach out to our support team.</p>
            <br/>
            <p>Best regards,</p>
            <p>The Admin Team</p>";
            }
            else
            {
                // For regular users
                return commonBody + $@"
            <p>You can now log in and start using our application with full access to the features.</p>
            <p>If you have any questions, feel free to reach out to our support team.</p>
            <br/>
            <p>Best regards,</p>
            <p>The Admin Team</p>";
            }
        }
    }
}

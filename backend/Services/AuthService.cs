// IT21470004 - BOPITIYA S. R. - Authentication Service

using backend.DTOs;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, EmailService emailService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<IdentityResult> Register(UserRegisterDTO userRegisterDTO)
        {
            bool isApproved = true; // Default to true
            bool needsAdminReview = false;

            // Check if the role requires admin approval
            if (userRegisterDTO.Role == "vendor" || userRegisterDTO.Role == "user")
            {
                isApproved = false;
                needsAdminReview = true;
            }

            // Create the user with the appropriate IsApproved flag
            var user = new User
            {
                Firstname = userRegisterDTO.Firstname,
                Lastname = userRegisterDTO.Lastname,
                Email = userRegisterDTO.Email,
                UserName = userRegisterDTO.Email,
                Phone = userRegisterDTO.Phone,
                Role = userRegisterDTO.Role,
                IsApproved = isApproved
            };

            // Create the user in the repository
            var result = await _userRepository.CreateUserAsync(user, userRegisterDTO.Password);

            // If the user was created successfully and needs admin review, send an email
            if (result.Succeeded && needsAdminReview)
            {
                var emailDTO = new EmailDTO
                {
                    ToEmail = userRegisterDTO.Email,
                    Subject = $"Registration as a {userRegisterDTO.Role}",
                    Body = $@"
            <h1>Welcome {userRegisterDTO.Firstname} {userRegisterDTO.Lastname},</h1>
            <p>We are excited to inform you that your registration request as a <strong>{userRegisterDTO.Role}</strong> has been received successfully.</p>
            <p>Your account is currently under review by our administration team for approval.</p>
            <p>You will receive an email once your registration has been reviewed and approved. Thank you for your patience.</p>
            <br/>
            <p>Best regards,</p>
            <p>The Admin Team</p>"
                };

                await SendEmail(emailDTO);
            }
            else if (result.Succeeded)
            {
                // Send a welcome email for automatically approved accounts
                var emailDTO = new EmailDTO
                {
                    ToEmail = userRegisterDTO.Email,
                    Subject = "Welcome! Your Account is Active",
                    Body = $@"
            <h1>Welcome {userRegisterDTO.Firstname} {userRegisterDTO.Lastname},</h1>
            <p>We are pleased to inform you that your account has been successfully created and is now active.</p>
            <p>You can now log in and start using our services.</p>
            <br/>
            <p>Best regards,</p>
            <p>The Team</p>"
                };

                await SendEmail(emailDTO);
            }

            return result;
        }


        public async Task<UserLoginResponseDTO> Login(UserLoginDTO userLoginDTO)
        {
            var user = await _userRepository.FindByEmailAsync(userLoginDTO.Email);

            if (user != null && await _userRepository.CheckPasswordAsync(user, userLoginDTO.Password))
            {
                // Check if the user is approved
                if (!user.IsApproved)
                {
                    // User is not approved, return a custom response
                    return new UserLoginResponseDTO
                    {
                        IsSuccess = false,
                        UserName = null,
                        Message = "Your registration is still pending admin validation. Please try again later or contact support for assistance."
                    };
                }

                var token = GenerateJwtToken(user);

                return new UserLoginResponseDTO
                {
                    IsSuccess = true,
                    UserId = user.Id.ToString(),
                    UserName = $"{user.Firstname} {user.Lastname}",
                    Role = user.Role,
                    Token = token
                };
            }

            // Invalid credentials
            return new UserLoginResponseDTO
            {
                IsSuccess = false,
                Message = "Invalid email or password. Please try again."
            };
        }

        public async Task Logout()
        {
            await _userRepository.Logout();
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<IdentityResult> UpdateUser(string userId, UserUpdateDTO userUpdateDTO)
        {
            var user = await _userRepository.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            user.Firstname = userUpdateDTO.Firstname ?? user.Firstname;
            user.Lastname = userUpdateDTO.Lastname ?? user.Lastname;
            user.Phone = userUpdateDTO.Phone ?? user.Phone;
            user.IsApproved = userUpdateDTO.IsApproved;

            return await _userRepository.UpdateUserAsync(user);
        }

        public async Task SendEmail(EmailDTO emailDTO)
        {
            await _emailService.SendEmailAsync(emailDTO);
        }


    }
}

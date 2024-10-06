using System;
using backend.DTOs;
using backend.Interfaces;
using Microsoft.AspNetCore.Identity;
using backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;



namespace backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<IdentityResult> Register(UserRegisterDTO userRegisterDTO)
        {
            var user = new User
            {
                Firstname = userRegisterDTO.Firstname,
                Lastname = userRegisterDTO.Lastname,
                Email = userRegisterDTO.Email,
                UserName = userRegisterDTO.Email,
                Phone = userRegisterDTO.Phone,
                Role = userRegisterDTO.Role,
                Status = Enum.TryParse(userRegisterDTO.Status, out UserStatus status) ? status : UserStatus.Deactive
            };

            return await _userRepository.CreateUserAsync(user, userRegisterDTO.Password);
        }

        // Login function
        [HttpPost("login")]
        public async Task<UserLoginResponseDTO> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var user = await _userRepository.FindByEmailAsync(userLoginDTO.Email);


            if (user != null && await _userRepository.CheckPasswordAsync(user, userLoginDTO.Password))
            {
                // return GenerateJwtToken(user);
                var token = GenerateJwtToken(user);

                // Return userId, role, and the generated JWT token
                return new UserLoginResponseDTO
                {
                    UserId = user.Id.ToString(),
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Role = user.Role,
                    Token = token,
                };
            }

            return null;

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
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<IdentityResult> UpdateUser(Guid userId, UserUpdateDTO userUpdateDTO)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            user.Firstname = userUpdateDTO.Firstname ?? user.Firstname;
            user.Lastname = userUpdateDTO.Lastname ?? user.Lastname;
            user.Phone = userUpdateDTO.Phone ?? user.Phone;

            if (Enum.TryParse(userUpdateDTO.Status, true, out UserStatus status))
            {
                user.Status = status;
            }

            return await _userRepository.UpdateUserAsync(user);
        }

    }
}

using System;
using backend.DTOs;
using backend.Interfaces;
using Microsoft.AspNetCore.Identity;
using backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;



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
                Status = Enum.TryParse(userRegisterDTO.Status, out UserStatus status) ? status : UserStatus.Deactive
            };

            return await _userRepository.CreateUserAsync(user, userRegisterDTO.Password);
        }

        public async Task<string> Login(UserLoginDTO userLoginDTO)
        {
            User user = await _userRepository.FindByEmailAsync(userLoginDTO.Email);

            if (user != null && await _userRepository.CheckPasswordAsync(user, userLoginDTO.Password))
            {
                return GenerateJwtToken(user);
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
                    new Claim(ClaimTypes.Email, user.Email)
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

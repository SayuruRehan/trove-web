// using System.Security.Cryptography;

// namespace backend.Utilities
// {
//     public static class PasswordGenerator
//     {
//         private const int PasswordLength = 12;

//         public static string GenerateRandomPassword()
//         {
//             const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
//             using (var rng = new RNGCryptoServiceProvider())
//             {
//                 var byteArray = new byte[PasswordLength];
//                 rng.GetBytes(byteArray);
//                 var result = new char[PasswordLength];
//                 for (int i = 0; i < PasswordLength; i++)
//                 {
//                     result[i] = validChars[byteArray[i] % validChars.Length];
//                 }
//                 return new string(result);
//             }
//         }
//     }
// }

using backend.Models;
using BCrypt.Net;

namespace backend.Utilities
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string inputPassword, string storedHashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHashedPassword);
        }

        internal static bool VerifyPassword(Vendor vendor, string hashedPassword)
        {
            throw new NotImplementedException();
        }
    }
}
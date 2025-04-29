using HealthyPawsV2.DAL;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace HealthyPawsV2.Utils
{
    public static class PasswordUtility
    {
        // Hash the password
        public static string HashPassword(ApplicationUser user, string password)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            return passwordHasher.HashPassword(user, password);
        }

        // Method to verify the hashed password
        public static bool VerifyPassword(ApplicationUser user, string hashedPassword, string passwordToCheck)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            var result = passwordHasher.VerifyHashedPassword(user, hashedPassword, passwordToCheck);
            return result == PasswordVerificationResult.Success;
        }
    }
}

using Authentication.Lib.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Lib.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser?> FindFirstUserAsync(this UserManager<AppUser> userManager)
        {
            if (userManager == null) throw new ArgumentNullException(nameof(userManager));

            return await userManager.Users.FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public static async Task<AppUser?> FindByEmailVerificationCodeAsync(this UserManager<AppUser> userManager,
            string emailVerificationCode)
        {
            if (userManager == null) throw new ArgumentNullException(nameof(userManager));
            if (string.IsNullOrEmpty(emailVerificationCode))
                throw new ArgumentException("Email verification code cannot be null or empty.",
                    nameof(emailVerificationCode));

            return await userManager.Users.FirstOrDefaultAsync(u => u.EmailVerificationCode == emailVerificationCode)
                .ConfigureAwait(false);
        }
    }
}

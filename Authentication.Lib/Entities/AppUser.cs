using Authentication.Lib.Enums;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Lib.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string? FullName { get; set; }
        public Country Country { get; set; }
        public string? EmailVerificationCode { get; set; }
    }
}

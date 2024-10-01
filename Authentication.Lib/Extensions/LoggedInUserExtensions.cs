using System.Security.Claims;

namespace Authentication.Lib.Extensions
{
    public static class LoggedInUserExtensions
    {
        public static Guid GetLoggedInUserId(this ClaimsPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) throw new InvalidOperationException("Kullanıcı ID'si bulunamadı.");

            return Guid.Parse(userId);
        }

        public static string GetLoggedInEmail(this ClaimsPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            var email = principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email)) throw new InvalidOperationException("Kullanıcı e-postası bulunamadı.");

            return email;
        }
    }
}

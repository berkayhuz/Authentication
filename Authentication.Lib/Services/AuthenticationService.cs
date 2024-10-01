using Authentication.Lib.Entities;
using Authentication.Lib.Enums;
using Authentication.Lib.Extensions.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Lib.Services
{
    public class AuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public AuthenticationService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<AuthenticationService> logger,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _configuration = configuration;
        }
        public async Task<string> GenerateJwtToken(AppUser user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            // Diğer claim'ler...
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationInMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        /// <summary>
        /// Kullanıcı kaydı yapar ve email doğrulama kodu gönderir.
        /// </summary>
        public async Task<IdentityResult> RegisterAsync(string email, string password, string fullName, Country country)
        {
            var user = new AppUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                Country = country,
                EmailVerificationCode = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                _logger.LogInformation("Kullanıcı başarıyla kaydedildi: {Email}", email);

                await SendVerificationEmailAsync(user.Email, user.EmailVerificationCode);
            }
            else
            {
                _logger.LogError("Kullanıcı kaydedilirken bir hata oluştu: {Email}", email);
            }

            return result;
        }

        /// <summary>
        /// Email doğrulama kodunu gönderir.
        /// </summary>
        private async Task SendVerificationEmailAsync(string email, string verificationCode)
        {
            var subject = "Email Doğrulama";
            var message = $"Lütfen email adresinizi doğrulamak için şu kodu kullanın: {verificationCode}";

            await _emailSender.SendEmailAsync(email, subject, message);
        }

        /// <summary>
        /// Kullanıcıyı oturum açar.
        /// </summary>
        public async Task<string> LoginAsync(string email, string password, bool isPersistent)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(email);
                return await GenerateJwtToken(user);
            }

            return null;
        }

        /// <summary>
        /// Kullanıcının oturumunu kapatır.
        /// </summary>
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Kullanıcı oturumu kapattı.");
        }

        /// <summary>
        /// Kullanıcı mevcut mu kontrol eder.
        /// </summary>
        public async Task<AppUser?> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Şifre sıfırlama işlemi için email doğrulama kodu gönderir.
        /// </summary>
        public async Task<string> GeneratePasswordResetTokenAsync(AppUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // Tokenı bir e-posta servisiyle kullanarak kullanıcıya gönderebilirsiniz
            return token;
        }

        /// <summary>
        /// Şifre sıfırlama işlemini gerçekleştirir.
        /// </summary>
        public async Task<IdentityResult> ResetPasswordAsync(AppUser user, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }
    }
}

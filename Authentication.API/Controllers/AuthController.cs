using Authentication.Lib.Services;
using Authentication.Lib.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;

        public AuthController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Kullanıcı kaydı yapar.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Lib.ViewModels.RegisterRequest request)
        {
            var result = await _authenticationService.RegisterAsync(request.Email, request.Password, request.FullName, request.Country);
            if (result.Succeeded)
            {
                return Ok(new { message = "Kullanıcı başarıyla kaydedildi." });
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Kullanıcı oturumu açar.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Lib.ViewModels.LoginRequest request)
        {
            var token = await _authenticationService.LoginAsync(request.Email, request.Password, request.IsPersistent);
            if (token != null)
            {
                return Ok(new { token });
            }

            return Unauthorized(new { message = "Geçersiz oturum açma bilgileri." });
        }

        /// <summary>
        /// Kullanıcı oturumunu kapatır.
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.LogoutAsync();
            return Ok(new { message = "Kullanıcı oturumu kapatıldı." });
        }

        /// <summary>
        /// Şifre sıfırlama talebi için email doğrulama kodu gönderir.
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] Lib.ViewModels.ResetPasswordRequest request)
        {
            var user = await _authenticationService.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Kullanıcı bulunamadı." });
            }

            var token = await _authenticationService.GeneratePasswordResetTokenAsync(user);
            // Tokenı bir e-posta servisiyle gönderin (ekstra işlem)
            return Ok(new { message = "Şifre sıfırlama kodu gönderildi.", token });
        }

        /// <summary>
        /// Şifre sıfırlama işlemini gerçekleştirir.
        /// </summary>
        [HttpPost("confirm-reset-password")]
        public async Task<IActionResult> ConfirmResetPassword([FromBody] ConfirmResetPasswordRequest request)
        {
            var user = await _authenticationService.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Kullanıcı bulunamadı." });
            }

            var result = await _authenticationService.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new { message = "Şifre başarıyla sıfırlandı." });
            }

            return BadRequest(result.Errors);
        }
    }
}

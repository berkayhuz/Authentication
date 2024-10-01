using Authentication.Lib.Enums;

namespace Authentication.Lib.ViewModels
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public Country Country { get; set; }
    }
}

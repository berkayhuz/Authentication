using Authentication.Lib.Entities;
using FluentValidation;

namespace Authentication.Lib.Extensions.FluentValidations
{
    public class UserValidator : AbstractValidator<AppUser>
    {
        public UserValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .MinimumLength(11)
                .MaximumLength(20)
                .WithName("Telefon numarası");

            RuleFor(x => x.EmailVerificationCode)
                .MinimumLength(4)
                .MaximumLength(6)
                .WithName("Email Doğrulama Kodu");
        }
    }
}

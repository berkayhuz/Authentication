using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Authentication.Lib.Extensions.FluentValidations
{
    public static class FluentValidationExtensions
    {
        public static void AddValidationResultToModelState(this ValidationResult validationResult,
            ModelStateDictionary modelState)
        {
            if (validationResult == null) throw new ArgumentNullException(nameof(validationResult));
            if (modelState == null) throw new ArgumentNullException(nameof(modelState));

            foreach (var error in validationResult.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }

        public static void AddIdentityResultToModelState(this IdentityResult identityResult,
            ModelStateDictionary modelState)
        {
            if (identityResult == null) throw new ArgumentNullException(nameof(identityResult));
            if (modelState == null) throw new ArgumentNullException(nameof(modelState));

            foreach (var error in identityResult.Errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}

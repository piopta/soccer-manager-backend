using FluentValidation;

namespace WebApi.Validation;

public class RegisterUserValidator : AbstractValidator<RegisterUser>
{
    public RegisterUserValidator()
    {
        RuleFor(r => r.Email).EmailAddress().NotNull();
        RuleFor(r => r.Password).MinimumLength(ApplicationConstants.Validation.MinimumPasswordLength).NotNull();
        RuleFor(r => r.ConfirmPassword).Equal(r => r.Password);
    }
}

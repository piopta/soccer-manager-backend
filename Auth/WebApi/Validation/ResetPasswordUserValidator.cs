using FluentValidation;

namespace WebApi.Validation;

public class ResetPasswordUserValidator : AbstractValidator<ResetPasswordUser>
{
    public ResetPasswordUserValidator()
    {
        RuleFor(r => r.Email).EmailAddress().NotNull();
        RuleFor(r => r.Password).MinimumLength(ApplicationConstants.Validation.MinimumPasswordLength).NotNull();
        RuleFor(r => r.ConfirmPassword).Equal(r => r.Password);
        RuleFor(r => r.Token).NotNull();
    }
}

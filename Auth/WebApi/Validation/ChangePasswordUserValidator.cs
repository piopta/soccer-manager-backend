using FluentValidation;

namespace WebApi.Validation;

public class ChangePasswordUserValidator : AbstractValidator<ChangePasswordUser>
{
    public ChangePasswordUserValidator()
    {
        RuleFor(r => r.Email).EmailAddress().NotNull();
        RuleFor(r => r.OldPassword).NotNull();
        RuleFor(r => r.NewPassword).MinimumLength(ApplicationConstants.Validation.MinimumPasswordLength).NotNull();
        RuleFor(r => r.ConfirmedNewPassword).Equal(r => r.NewPassword);
    }
}

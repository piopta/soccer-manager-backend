using FluentValidation;

namespace WebApi.Validation;

public class ValidateTokenValidator : AbstractValidator<ValidateToken>
{
    public ValidateTokenValidator()
    {
        RuleFor(v => v.Token).NotNull();
    }
}
